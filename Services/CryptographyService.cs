using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CryptographyStream = System.Security.Cryptography.CryptoStream;

namespace CryptoStream.Services
{
    public class CryptographyService
    {
        private readonly UTF7Encoding utf7Encoding = new UTF7Encoding();

        private SymmetricAlgorithm algorithm { get; set; }
        private byte[] key { get; set; }
        private byte[] initVector { get; set; }

        public CryptographyService(SymmetricAlgorithm algorithm)
        {
            this.algorithm = algorithm;
            key = algorithm.Key;
            initVector = algorithm.IV;
        }

        public string Encrypt(string text)
        {
            byte[] input = utf7Encoding.GetBytes(text);
            byte[] output = Transform(input, algorithm.CreateEncryptor(key, initVector));
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input, algorithm.CreateDecryptor(key, initVector));
            return utf7Encoding.GetString(output);
        }

        public byte[] Encrypt(byte[] input)
        {
            return Transform(input, algorithm.CreateEncryptor(key, initVector));
        }

        public byte[] Decrypt(byte[] input)
        {
            return Transform(input, algorithm.CreateDecryptor(key, initVector));
        }

        private byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptographyStream cryptoStream = new CryptographyStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(input, 0, input.Length);
                    cryptoStream.FlushFinalBlock();

                    byte[] result = memoryStream.ToArray();
                    return result;
                }
            }
        }
    }
}