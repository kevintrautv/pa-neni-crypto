using System;
using System.IO;
using System.Text;
using crypto.Core.Cryptography;
using crypto.Core.Extension;
using crypto.Core.Recources;

namespace crypto.Core.Header
{
    public static class VaultHeaderReader
    {
        public static VaultHeader ReadFrom(Stream source)
        {
            using var binReader = new BinaryReader(source, Encoding.Unicode, true);

            var magicNumber = binReader.ReadBytes(VaultHeader.MagicNumberLength);

            if (!magicNumber.ContentEqualTo(VaultHeader.MagicNumber))
                throw new FormatException(Strings.VaultHeaderReader_ReadFrom_Magic_number_doesn_t_match);

            var result = new VaultHeader();

            var mpAuthentication = binReader.ReadBytes(AesSizes.Auth);
            var encryptedMp = binReader.ReadBytes(AesSizes.Key);

            result.MasterPassword = new MasterPassword(mpAuthentication, encryptedMp);

            return result;
        }
    }
}