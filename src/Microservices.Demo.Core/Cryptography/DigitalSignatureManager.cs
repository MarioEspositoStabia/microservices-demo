using Newtonsoft.Json;
using PemUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Microservices.Demo.Core.Cryptography
{
    public static class DigitalSignatureManager
    {
        private const int KeySize = 2048;
        private static readonly Encoding _encoder = Encoding.UTF8;

        public static RSAKeyPairModel GenerateKeyPairs()
        {
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(KeySize))
            {
                rsaCryptoServiceProvider.PersistKeyInCsp = false;
                RSAParameters privateKeyRSA = rsaCryptoServiceProvider.ExportParameters(true);
                RSAParameters publicKeyRSA = rsaCryptoServiceProvider.ExportParameters(false);
                RSAKeyPairModel keyPair = new RSAKeyPairModel(privateKeyRSA, publicKeyRSA);

                return keyPair;
            }
        }

        public static RSAParameters GetKey(string key)
        {
            byte[] jsonPublicKeyBytes = Convert.FromBase64String(key);
            string jsonPublicKey = _encoder.GetString(jsonPublicKeyBytes);
            RSAParametersModel rsaParametersModel = JsonConvert.DeserializeObject<RSAParametersModel>(jsonPublicKey);
            return rsaParametersModel.ToRSAParameters();
        }

        public static RSAParameters GetPem(string pemKey)
        {
            RSAParameters key;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(pemKey);
                    streamWriter.Flush();
                    memoryStream.Position = 0;
                    using (PemReader reader = new PemReader(memoryStream))
                    {
                        key = reader.ReadRsaKey();
                    }
                }
            }

            return key;
        }

        public static byte[] SignData(byte[] bytesOfData, string privateKey)
        {
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(KeySize))
            {
                rsaCryptoServiceProvider.PersistKeyInCsp = false;
                rsaCryptoServiceProvider.ImportParameters(GetKey(privateKey));

                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsaCryptoServiceProvider);
                rsaFormatter.SetHashAlgorithm("SHA256");

                byte[] hashBytesOfDataToSign = bytesOfData.ComputeHashSHA256();
                return rsaFormatter.CreateSignature(hashBytesOfDataToSign);
            }
        }

        public static string SignData(string data, string privateKey)
        {
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(KeySize))
            {
                rsaCryptoServiceProvider.PersistKeyInCsp = false;
                rsaCryptoServiceProvider.ImportParameters(GetKey(privateKey));

                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsaCryptoServiceProvider);
                rsaFormatter.SetHashAlgorithm("SHA256");

                byte[] bytesOfData = _encoder.GetBytes(data);
                byte[] hashBytesOfDataToSign = bytesOfData.ComputeHashSHA256();
                byte[] signature = rsaFormatter.CreateSignature(hashBytesOfDataToSign);
                return Convert.ToBase64String(signature);
            }
        }

        public static bool VerifySignature(byte[] bytesOfData, byte[] signatureBytes, string publicKey)
        {
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(KeySize))
            {
                rsaCryptoServiceProvider.ImportParameters(GetKey(publicKey));
                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsaCryptoServiceProvider);
                rsaDeformatter.SetHashAlgorithm("SHA256");
                byte[] hashBytesOfDataToSign = bytesOfData.ComputeHashSHA256();
                return rsaDeformatter.VerifySignature(hashBytesOfDataToSign, signatureBytes);
            }
        }

        public static bool VerifySignature(string data, string signature, string publicKey)
        {
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(KeySize))
            {
                rsaCryptoServiceProvider.ImportParameters(GetKey(publicKey));
                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsaCryptoServiceProvider);
                rsaDeformatter.SetHashAlgorithm("SHA256");
                byte[] bytesOfData = _encoder.GetBytes(data);
                byte[] hashBytesOfDataToSign = bytesOfData.ComputeHashSHA256();
                byte[] signatureBytes = Convert.FromBase64String(signature);
                return rsaDeformatter.VerifySignature(hashBytesOfDataToSign, signatureBytes);
            }
        }
    }
}
