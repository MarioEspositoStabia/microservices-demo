using System.ComponentModel.DataAnnotations;

namespace Microservices.Demo.Core.MVC
{
    public class RefreshTokenModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
