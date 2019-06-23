using System.Threading.Tasks;

namespace Microservices.Demo.Core.MVC.Utils
{
    public interface IEmailHandler
    {
        Task SendMailAsync(string subject, string body, string to, params string[] toParameters);
    }
}
