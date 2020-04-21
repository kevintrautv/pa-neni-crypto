using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using crypto.Core.Exceptions;
using crypto.Core.Extension;
using crypto.Core.Header;
using crypto.Core.Recources;

namespace crypto.Core
{
    public class Vault : IDisposable
    {
        private const string FileExtension = ".vlt";
        private const string UnlockedFolderName = "Unlocked";
        private const string EncryptedFolderName = "Encrypted";
        private readonly byte[] _key;

        internal Vault(string name, byte[] key)
        {
            Name = name;
            _key = key;
        }

        private string Name { get; }
        public VaultHeader Header { get; set; }
        public ConcurrentBag<UserDataFile> UserDataFiles { get; } = new ConcurrentBag<UserDataFile>();
        public string VaultPath { get; set; }
        public string EncryptedFolderPath => Path.Combine(VaultPath, EncryptedFolderName);
        public string UnlockedFolderPath => Path.Combine(VaultPath, UnlockedFolderName);
        public string VaultFilePath => GetVaultFilePath(VaultPath, Name);

        public void Dispose()
        {
            VaultReaderWriter.WriteConfig(this, _key);
        }

        internal static string GetVaultFilePath(string vaultPath, string name)
        {
            return vaultPath + "/" + name + FileExtension;
        }

        public static Vault Create(string name, byte[] key, string path = null)
        {
            var output = new Vault(name, key)
            {
                Header = VaultHeader.Create(),
                VaultPath = path == null
                    ? Path.Combine(Environment.CurrentDirectory, name)
                    : Path.GetFullPath(path + "/" + name)
            };

            PrepareVault(output);

            return output;
        }

        public static Vault Open(VaultPaths folderPath, byte[] key)
        {
            return VaultReaderWriter.ReadFromConfig(folderPath, key);
        }

        public async Task AddFileAsync(string sourcePath, string path = "")
        {
            if (!File.Exists(sourcePath)) throw new FileNotFoundException(Strings.Vault_AddFileAsync_File_not_found, sourcePath);

            var name = Path.GetFileName(sourcePath);
            var newFile = new UserDataFile(UserDataHeader.Create(name, path));

            if (PlainNameAlreadyExists(newFile.Header.SecuredPlainName.PlainName)) FileAlreadyExists();

            var destinationPath = Path.Combine(EncryptedFolderPath, newFile.Header.TargetPath);
            await WriteDecryptedAsync(newFile, sourcePath, destinationPath);

            UserDataFiles.Add(newFile);
        }

        public async Task RemoveFile(UserDataFile file)
        {
            var success = UserDataFiles.TryTake(out file);
            if (!success) throw new FileNotFoundException(Strings.Vault_RemoveFile_Couldn_t_take_out_file_);
            File.Delete(Path.Combine(EncryptedFolderPath, file.Header.TargetPath));
            if (file.Header.IsUnlocked) await EliminateExtracted(file);
        }

        public void RenameFile(UserDataFile file, string name)
        {
            if (name.Contains("/")) throw new NotANameException(Strings.Vault_RenameFile_Argument_isn_t_a_name);
            var dir = NDirectory.GetPathParentDir(file.Header.SecuredPlainName.PlainName);
            MoveFile(file, dir + name);
        }

        public void MoveFile(UserDataFile file, string destination)
        {
            // TODO: check if destination is a path by ending with a /
            var relativePath = NPath.RemoveRelativeParts(destination);
            var prevFileName = file.Header.SecuredPlainName.PlainName;

            if (PlainNameAlreadyExists(relativePath)) FileAlreadyExists();

            file.Move(relativePath);

            if (file.Header.IsUnlocked)
            {
                var srcPath = Path.Combine(UnlockedFolderPath, prevFileName);
                var destPath = Path.Combine(UnlockedFolderPath, file.Header.SecuredPlainName.PlainName);
                NDirectory.CreateMissingDirs(destPath);
                File.Move(srcPath, destPath);
            }
        }

        public async Task<ExtractStatus> ExtractFile(UserDataFile file)
        {
            FixItemHeaderForUnlockedFile(file);

            var encryptedSourcePath = Path.Combine(EncryptedFolderPath, file.Header.TargetPath);
            var unlockedTarget = UserDataPathToUnlocked(file);

            if (file.Header.IsUnlocked) return ExtractStatus.Duplicate;

            var hash = await UserDataFile.ExtractUserDataFile(encryptedSourcePath, unlockedTarget,
                Header.MasterPassword.Password, file.Header.TargetCipherIV);

            var now = DateTime.Now;
            File.SetLastWriteTime(encryptedSourcePath, now);
            File.SetLastWriteTime(unlockedTarget, now);

            file.Header.IsUnlocked = true;

            return hash.ContentEqualTo(file.Header.TargetAuthentication) ? ExtractStatus.Ok : ExtractStatus.HashNoMatch;
        }

        public string UserDataPathToUnlocked(UserDataFile file)
        {
            return Path.Combine(UnlockedFolderPath, file.Header.SecuredPlainName.PlainName);
        }
        
        public string UserDataPathToEncrypted(UserDataFile file)
        {
            return Path.Combine(EncryptedFolderPath, file.Header.TargetPath);
        }

        public async Task EliminateExtracted(UserDataFile file)
        {
            if (!file.Header.IsUnlocked) throw new FileNotUnlockedException();
            var plainTextPath = file.Header.SecuredPlainName.PlainName;

            var path = Path.Combine(UnlockedFolderPath, plainTextPath);

            if (!File.Exists(path))
            {
                file.Header.IsUnlocked = false;
                throw new FileNotFoundException(Strings.Vault_EliminateExtracted_Decrypted_file_was_not_found, plainTextPath);
            }

            await NFile.Purge(path);
            file.Header.IsUnlocked = false;

            var parentDir = Path.Combine(UnlockedFolderPath, NDirectory.GetPathParentDir(plainTextPath));
            NDirectory.DeleteDirIfEmpty(parentDir, UnlockedFolderName);
        }

        public UserDataFile GetFileByPath(string path)
        {
            foreach (var file in UserDataFiles)
                if (file.Header.SecuredPlainName.PlainName == path)
                    return file;

            return null;
        }

        public void CheckAndCorrectAllItemHeaders()
        {
            foreach (var itemHeader in UserDataFiles) FixItemHeaderForUnlockedFile(itemHeader);
        }

        private static void PrepareVault(Vault vaultFile)
        {
            Directory.CreateDirectory(vaultFile.VaultPath);
            Directory.CreateDirectory(vaultFile.EncryptedFolderPath);
            Directory.CreateDirectory(vaultFile.UnlockedFolderPath);
            File.Create(vaultFile.VaultFilePath).Dispose();
        }

        private static void FileAlreadyExists()
        {
            throw new FileAlreadyExistsException(Strings.Vault_FileAlreadyExists_File_already_exists_in_Vault);
        }

        public async Task WriteDecryptedAsync(UserDataFile file, string sourcePath, string destinationPath)
        {
            var hash = await UserDataFile.WriteUserDataFileAsync(sourcePath, destinationPath,
                Header.MasterPassword.Password, file.Header.TargetCipherIV);

            file.Header.TargetAuthentication = hash;
        }

        private bool PlainNameAlreadyExists(string plainName)
        {
            foreach (var vltFile in UserDataFiles)
                if (vltFile.Header.SecuredPlainName.PlainName == plainName)
                    return true;

            return false;
        }

        private void FixItemHeaderForUnlockedFile(UserDataFile file)
        {
            if (ItemHeaderIsMissingUnlockedFile(file)) file.Header.IsUnlocked = false;
        }

        private bool ItemHeaderIsMissingUnlockedFile(UserDataFile item)
        {
            if (!item.Header.IsUnlocked) return true;

            return !File.Exists(Path.Combine(UnlockedFolderPath, item.Header.SecuredPlainName.PlainName));
        }
    }
}