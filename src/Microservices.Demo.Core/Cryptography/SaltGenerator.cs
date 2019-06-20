using System;
using System.Security.Cryptography;

namespace Microservices.Demo.Core.Cryptography
{
    public static class SaltGenerator
    {
        public static string Generate()
        {
            string token;
            using (RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                randomNumberGenerator.GetBytes(tokenData);

                token = Convert.ToBase64String(tokenData);
            }

            return token;
        }
    }
}
