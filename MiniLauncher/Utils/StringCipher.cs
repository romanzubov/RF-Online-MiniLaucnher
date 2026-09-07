using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MiniLauncher.Utils
{
    /// <summary>
    /// AES-256-CBC с PBKDF2 (Rfc2898). Формат: [32 байта соли] + [16 байт IV] + [шифртекст].
    /// Раньше использовался RijndaelManaged с BlockSize=256 — .NET Core/.NET 5+ поддерживает
    /// только 128-битный блок (PlatformNotSupportedException), поэтому формат изменён.
    /// </summary>
    public static class StringCipher
    {
        private const int KeySize = 256;
        private const int SaltSize = 32;
        private const int IvSize = 16;
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            var salt = RandomBytes(SaltSize);
            var iv = RandomBytes(IvSize);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (var kdf = new Rfc2898DeriveBytes(passPhrase, salt, DerivationIterations))
                using (var encryptor = aes.CreateEncryptor(kdf.GetBytes(KeySize / 8), iv))
                using (var ms = new MemoryStream())
                {
                    ms.Write(salt, 0, salt.Length);
                    ms.Write(iv, 0, iv.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            var all = Convert.FromBase64String(cipherText);
            if (all.Length < SaltSize + IvSize + 16)
                throw new CryptographicException("Cipher text is too short or in an unsupported (legacy) format.");

            var salt = new byte[SaltSize];
            var iv = new byte[IvSize];
            Buffer.BlockCopy(all, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(all, SaltSize, iv, 0, IvSize);
            int cipherLen = all.Length - SaltSize - IvSize;

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (var kdf = new Rfc2898DeriveBytes(passPhrase, salt, DerivationIterations))
                using (var decryptor = aes.CreateDecryptor(kdf.GetBytes(KeySize / 8), iv))
                using (var ms = new MemoryStream(all, SaltSize + IvSize, cipherLen))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static byte[] RandomBytes(int count)
        {
            var bytes = new byte[count];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }
    }
}
