namespace Microservices.Demo.Core.MVC.Authentication
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpiryMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
