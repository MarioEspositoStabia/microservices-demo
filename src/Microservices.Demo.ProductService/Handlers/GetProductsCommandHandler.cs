using Microservices.Demo.Core.Commands;
using Microservices.Demo.Core.MVC.Authentication;
using Microservices.Demo.ProductService.Database.Documents;
using Microservices.Demo.ProductService.Messaging.Commands;
using Microservices.Demo.ProductService.Messaging.Events;
using Microservices.Demo.ProductService.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Demo.ProductService.Handlers
{
    public class GetProductsCommandHandler : ICommandHandler<GetProductsCommand>
    {
        private readonly IBusClient _busClient;
        private readonly IProductService _productService;
        private readonly JwtOptions _jwtOptions;

        public GetProductsCommandHandler(IBusClient busClient, IProductService productService, IOptions<JwtOptions> options)
        {
            this._busClient = busClient;
            this._productService = productService;
            this._jwtOptions = options.Value;
        }

        public async Task HandleAsync(GetProductsCommand command)
        {
            try
            {
                TokenManager.GetPrincipalFromExpiredToken(command.Token, this._jwtOptions.PublicKey, this._jwtOptions.Issuer, this._jwtOptions.Audience);

                IAsyncCursor<Product> productsCursor = await this._productService.GatAllProductsAsync();

                List<Product> products = productsCursor.ToList();

                string jsonProducts = JsonConvert.SerializeObject(products);

                await _busClient.PublishAsync(new GetProductsEvent(jsonProducts)
                {
                    ConnectionId = command.ConnectionId
                });
            }
            catch (Exception ex)
            {
                await _busClient.PublishAsync(new GetProductsRejectedEvent(ex.Message, "error")
                {
                    ConnectionId = command.ConnectionId
                });
            }
        }
    }
}
