using Microservices.Demo.Core.MVC;
using Microservices.Demo.IdentityService.Messaging.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using System.Threading.Tasks;

namespace Micorservices.Demo.ApiGateway.Controllers
{
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IBusClient _busClient;

        public UsersController(IBusClient busClient, LocalizationService localizationService) : base(localizationService)
        {

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand createUserCommand)
        {
            await _busClient.PublishAsync(createUserCommand);

            return Accepted();
        }
    }
}
