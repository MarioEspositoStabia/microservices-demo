namespace Microservices.Demo.Core.MVC.Utils
{
    public class EmailOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
