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
            this._busClient = busClient;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand createUserCommand)
        {
            await this._busClient.PublishAsync(createUserCommand);

            return Accepted();
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] GetTokenCommand getTokenCommand)
        {
            await this._busClient.PublishAsync(getTokenCommand);

            return Accepted();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand refreshTokenCommand)
        {
            await this._busClient.PublishAsync(refreshTokenCommand);

            return Accepted();
        }
    }
}
