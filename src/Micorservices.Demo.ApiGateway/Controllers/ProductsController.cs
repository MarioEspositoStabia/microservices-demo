using Microservices.Demo.Core.MVC;
using Microservices.Demo.ProductService.Messaging.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using System.Threading.Tasks;

namespace Micorservices.Demo.ApiGateway.Controllers
{
    [Route("api/products")]
    public class ProductsController : BaseController
    {
        private readonly IBusClient _busClient;

        public ProductsController(IBusClient busClient, LocalizationService localizationService) : base(localizationService)
        {
            this._busClient = busClient;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts([FromBody] GetProductsCommand getProductsCommand)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            getProductsCommand.Token = accessToken;

            await this._busClient.PublishAsync(getProductsCommand);

            return Accepted();
        }
    }
}
