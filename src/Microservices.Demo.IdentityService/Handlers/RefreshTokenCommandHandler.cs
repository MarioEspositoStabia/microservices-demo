using Microservices.Demo.Core.Commands;
using Microservices.Demo.Core.MVC;
using Microservices.Demo.Core.MVC.Authentication;
using Microservices.Demo.IdentityService.Messaging.Commands;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microservices.Demo.IdentityService.Services;
using Microsoft.Extensions.Options;
using RawRabbit;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Microservices.Demo.IdentityService.Handlers
{
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand>
    {
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly JwtOptions _jwtOptions;
        private readonly LocalizationService _localizationService;

        public RefreshTokenCommandHandler(IBusClient busClient, IUserService userService, IConnectionMultiplexer connectionMultiplexer, IOptions<JwtOptions> options, LocalizationService localizationService)
        {
            this._busClient = busClient;
            this._userService = userService;
            this._connectionMultiplexer = connectionMultiplexer;
            this._jwtOptions = options.Value;
            this._localizationService = localizationService;
        }

        public async Task HandleAsync(RefreshTokenCommand command)
        {
            RefreshTokenModel newToken;

            try
            {
                if (_jwtOptions.ExpiryMinutes != 0)
                {
                    newToken = TokenManager.RefreshToken(this._connectionMultiplexer, command.Token, command.RefreshToken, _jwtOptions.PrivateKey, _jwtOptions.PublicKey, _jwtOptions.Issuer, _jwtOptions.Audience, _jwtOptions.ExpiryMinutes);
                }
                else
                {
                    newToken = TokenManager.RefreshToken(this._connectionMultiplexer, command.Token, command.RefreshToken, _jwtOptions.PrivateKey, _jwtOptions.PublicKey, _jwtOptions.Issuer, _jwtOptions.Audience);
                }

                await _busClient.PublishAsync(new RefreshTokenEvent(newToken)
                {
                    ConnectionId = command.ConnectionId
                });
            }
            catch (Exception ex)
            {
                await _busClient.PublishAsync(new RefreshTokenRejectedEvent(ex.Message, "error")
                {
                    ConnectionId = command.ConnectionId
                });
                throw;
            }
            
        }
    }
}
