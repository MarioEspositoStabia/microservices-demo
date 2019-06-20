using System;
using System.Security.Cryptography;

namespace Microservices.Demo.Core.Cryptography
{
    public class RSAParametersModel
    {
        public string D { get; set; }

        public string DP { get; set; }

        public string DQ { get; set; }

        public string Exponent { get; set; }

        public string InverseQ { get; set; }

        public string Modulus { get; set; }

        public string P { get; set; }

        public string Q { get; set; }

        public static RSAParametersModel ConvertFromRSAParameters(RSAParameters rsaParameters)
        {
            return new RSAParametersModel()
            {
                D = rsaParameters.D == null ? null : Convert.ToBase64String(rsaParameters.D),
                DP = rsaParameters.DP == null ? null : Convert.ToBase64String(rsaParameters.DP),
                DQ = rsaParameters.DQ == null ? null : Convert.ToBase64String(rsaParameters.DQ),
                Exponent = Convert.ToBase64String(rsaParameters.Exponent),
                InverseQ = rsaParameters.InverseQ == null ? null : Convert.ToBase64String(rsaParameters.InverseQ),
                Modulus = Convert.ToBase64String(rsaParameters.Modulus),
                P = rsaParameters.P == null ? null : Convert.ToBase64String(rsaParameters.P),
                Q = rsaParameters.Q == null ? null : Convert.ToBase64String(rsaParameters.Q)
            };
        }

        public RSAParameters ToRSAParameters()
        {
            return new RSAParameters()
            {
                D = this.D == null ? null : Convert.FromBase64String(this.D),
                DP = this.DP == null ? null : Convert.FromBase64String(this.DP),
                DQ = this.DQ == null ? null : Convert.FromBase64String(this.DQ),
                Exponent = Convert.FromBase64String(this.Exponent),
                InverseQ = this.InverseQ == null ? null : Convert.FromBase64String(this.InverseQ),
                Modulus = Convert.FromBase64String(this.Modulus),
                P = this.P == null ? null : Convert.FromBase64String(this.P),
                Q = this.Q == null ? null : Convert.FromBase64String(this.Q)
            };
        }
    }
}
