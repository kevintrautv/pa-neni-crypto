using System.IO;
using System.Threading.Tasks;
using crypto.Core;

namespace crypto.Desktop.Cnsl.Commands
{
    public class MoveCommand : CommandAsync
    {
        public string? VaultPath { get; }
        public string? OldName { get; }
        public string? NewName { get; }

        public MoveCommand(string? vaultPath, string? oldName, string? newName)
        {
            VaultPath = vaultPath ?? throw new NoConsoleArgumentException("No path to vault given");
            OldName = oldName ?? throw new NoConsoleArgumentException("No path to file given");
            NewName = newName ?? throw new NoConsoleArgumentException("No new path given");
        }

        public override Task Run()
        {
            using var vault = StandardVault.Generate(VaultPath);

            var file = vault.GetFileByPath(OldName);

            if (file == null)
            {
                throw new FileNotFoundException("The file to rename wasn't found");
            }

            vault.MoveFile(file, NewName);
            
            Notifier.Success("File moved to " + NewName);
            return Task.CompletedTask;
        }
    }
}