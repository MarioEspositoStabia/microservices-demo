using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Microservices.Demo.Core.Cryptography
{
    public static class Extensions
    {
        private const int PEPPER_KEY = 811086;

        private static SHA256CryptoServiceProvider _sha256Hasher;
        private static UTF8Encoding _utf8Encoder;
        private static Random _random;

        static Extensions()
        {
            _sha256Hasher = new SHA256CryptoServiceProvider();
            _utf8Encoder = new UTF8Encoding();
            _random = new Random(PEPPER_KEY);
        }

        public static byte[] ComputeHashSHA256(this byte[] bytes)
        {
            byte[] hashedDataBytes = _sha256Hasher.ComputeHash(bytes);
            return hashedDataBytes;
        }

        public static string ComputeHashSHA256(this string phrase)
        {
            byte[] hashedDataBytes = _sha256Hasher.ComputeHash(_utf8Encoder.GetBytes(phrase));
            return Convert.ToBase64String(hashedDataBytes);
        }

        public static string ComputeHashSHA256(this string phrase, string salt)
        {
            if (string.IsNullOrWhiteSpace(salt))
            {
                return phrase.ComputeHashSHA256();
            }

            string pepper = GeneratePepper(phrase, salt);

            byte[] phraseBytes = _utf8Encoder.GetBytes(phrase);
            string phraseBase64 = Convert.ToBase64String(phraseBytes);

            byte[] saltBytes = _utf8Encoder.GetBytes(salt);
            string saltBase64 = Convert.ToBase64String(saltBytes);

            byte[] pepperBytes = _utf8Encoder.GetBytes(pepper);
            string pepperBase64 = Convert.ToBase64String(pepperBytes);

            byte[] bytes = _utf8Encoder.GetBytes(phraseBase64 + saltBase64 + pepperBytes);

            byte[] hashedDataBytes = _sha256Hasher.ComputeHash(bytes);
            return Convert.ToBase64String(hashedDataBytes);
        }

        private static string GeneratePepper(string phrase, string salt)
        {
            string temp = phrase + salt;
            return Shuffle(temp, PEPPER_KEY);
        }

        private static string Shuffle(string source, int randKey)
        {
            char[] chars = source.ToArray();
            char swap;

            for (int charIdx = chars.Length - 1; charIdx > 0; charIdx--)
            {
                int n = _random.Next(charIdx);
                swap = chars[n];
                chars[n] = chars[charIdx];
                chars[charIdx] = swap;
            }

            return new string(chars);
        }
    }
}
