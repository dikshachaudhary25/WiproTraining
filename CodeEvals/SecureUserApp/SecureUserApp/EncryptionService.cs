using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace SecureUserApp
{
    public class EncryptionService
    {
        private readonly byte[] key = Encoding.UTF8.GetBytes("1234567890123456");
        private readonly byte[] iv = Encoding.UTF8.GetBytes("6543210987654321");
        public string Encrypt(string plainText)
        {
            try
            {
                using Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor();
                using MemoryStream ms = new MemoryStream();
                using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using StreamWriter sw = new StreamWriter(cs);

                sw.Write(plainText);
                sw.Flush();
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Encryption failed");
                throw;
            }
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                using Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor();

                using MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText));
                using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using StreamReader sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Decryption failed");
                throw;
            }
        }
    }
}