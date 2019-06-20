using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Microservices.Demo.Core.Cryptography
{
    public class RSAKeyPairModel
    {
        private readonly Encoding _encoder = Encoding.UTF8;
        private readonly RSAParametersModel _privateKey;
        private readonly RSAParametersModel _publicKey;

        public RSAKeyPairModel(RSAParameters privateKey, RSAParameters publicKey)
        {
            if (privateKey.Equals(default(RSAParameters)) || publicKey.Equals(default(RSAParameters)))
            {
                throw new ArgumentException();
            }

            this._privateKey = RSAParametersModel.ConvertFromRSAParameters(privateKey);
            this._publicKey = RSAParametersModel.ConvertFromRSAParameters(publicKey);
        }

        public string PrivateKey
        {
            get
            {
                return this.GetKey(this._privateKey);
            }
        }

        public string PublicKey
        {
            get
            {
                return this.GetKey(this._publicKey);
            }
        }

        private string GetKey(RSAParametersModel key)
        {
            string serializedKey = JsonConvert.SerializeObject(key);
            byte[] keyBytes = this._encoder.GetBytes(serializedKey);
            return Convert.ToBase64String(keyBytes);
        }
    }
}
