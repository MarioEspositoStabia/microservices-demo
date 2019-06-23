using Microservices.Demo.Core.Commands;
using Microservices.Demo.Core.Cryptography;
using Microservices.Demo.Core.Enumerations;
using Microservices.Demo.Core.MVC;
using Microservices.Demo.Core.MVC.Authentication;
using Microservices.Demo.Core.MVC.Utils;
using Microservices.Demo.IdentityService.Database.Entity;
using Microservices.Demo.IdentityService.Messaging.Commands;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microservices.Demo.IdentityService.Services;
using Microsoft.Extensions.Options;
using RawRabbit;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Demo.IdentityService.Handlers
{
    public class GetTokenCommandHandler : ICommandHandler<GetTokenCommand>
    {
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;
        private readonly IEmailHandler _emailHandler;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly JwtOptions _jwtOptions;
        private readonly LocalizationService _localizationService;
        private StringBuilder _stringBuilder = new StringBuilder(500);

        public GetTokenCommandHandler(IConnectionMultiplexer connectionMultiplexer, IBusClient busClient, IUserService userService, IEmailHandler emailHandler, IOptions<JwtOptions> jwtOptions, LocalizationService localizationService)
        {
            this._connectionMultiplexer = connectionMultiplexer;
            this._busClient = busClient;
            this._userService = userService;
            this._emailHandler = emailHandler;
            this._jwtOptions = jwtOptions.Value;
            this._localizationService = localizationService;
        }

        public async Task HandleAsync(GetTokenCommand command)
        {
            try
            {
                User user = null;
                if (!string.IsNullOrWhiteSpace(command.UserNameOrEmail))
                {
                    user = await this._userService.GetByUserNameAsync(command.UserNameOrEmail);

                    if (user == null)
                    {
                        user = await this._userService.GetByEmailAsync(command.UserNameOrEmail);
                    }
                }
                StatusCode statusCode = null;
                ApiError apiError = null;
                if (user == null)
                {
                    statusCode = this._localizationService.GetLocalizedStatusCode(ResponseCode.UserNameExists);
                    apiError = new ApiError(statusCode);

                    await _busClient.PublishAsync(new GetTokenRejectedEvent(command.UserNameOrEmail, apiError, statusCode.Code)
                    {
                        ConnectionId = command.ConnectionId
                    });

                    return;
                }

                if (!user.EmailConfirmed)
                {
                    await _busClient.PublishAsync(new GetTokenRejectedEvent(command.UserNameOrEmail, apiError, statusCode.Code)
                    {
                        ConnectionId = command.ConnectionId
                    });

                    return;
                }

                if (user.IsLockedOut)
                {
                    statusCode = this._localizationService.GetLocalizedStatusCode(ResponseCode.UserLocked);
                    apiError = new ApiError(statusCode);

                    await _busClient.PublishAsync(new GetTokenRejectedEvent(command.UserNameOrEmail, apiError, statusCode.Code)
                    {
                        ConnectionId = command.ConnectionId
                    });

                    return;
                }

                if (!user.ValidateUser(command.Password))
                {
                    if (user.FailedPasswordAttempt.GetValueOrDefault() == 0)
                    {
                        user.FailedPasswordAttemptDate = DateTimeOffset.UtcNow;
                    }
                    else if (user.FailedPasswordAttemptDate.GetValueOrDefault() > DateTimeOffset.UtcNow.AddMinutes(-10))
                    {
                        user.FailedPasswordAttempt = 0;
                        user.FailedPasswordAttemptDate = DateTimeOffset.UtcNow;
                    }

                    user.FailedPasswordAttempt = user.FailedPasswordAttempt.GetValueOrDefault() + 1;

                    await this._userService.UpdateAsync(user, user.Id);

                    statusCode = this._localizationService.GetLocalizedStatusCode(ResponseCode.UnknownUser);
                    apiError = new ApiError(statusCode);

                    await _busClient.PublishAsync(new GetTokenRejectedEvent(command.UserNameOrEmail, apiError, statusCode.Code)
                    {
                        ConnectionId = command.ConnectionId
                    });

                    return;
                }
                else
                {
                    if (user.FailedPasswordAttempt > 5)
                    {
                        user.IsLockedOut = true;
                        user.LastLockOutDate = DateTimeOffset.UtcNow;
                        user.VerificationCode = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
                        await this._userService.UpdateAsync(user, user.Id);

                        string verificationMail = $@"/api/auth/unlockUser?code={user.VerificationCode}";

                        _stringBuilder.Clear();
                        _stringBuilder.AppendLine($"Hi {user.UserName},");
                        _stringBuilder.AppendLine();
                        _stringBuilder.AppendLine("Account locked.");
                        _stringBuilder.AppendLine();
                        _stringBuilder.AppendLine("In order to reactivate your account please click the link below: ");
                        _stringBuilder.AppendLine(verificationMail);
                        _stringBuilder.AppendLine();
                        _stringBuilder.AppendLine("Thanks,");
                        _stringBuilder.AppendLine("Microservices Demo");

                        await _emailHandler.SendMailAsync("User Locked", _stringBuilder.ToString(), user.Email);

                        statusCode = this._localizationService.GetLocalizedStatusCode(ResponseCode.UserLocked);
                        apiError = new ApiError(statusCode);

                        await _busClient.PublishAsync(new GetTokenRejectedEvent(command.UserNameOrEmail, apiError, statusCode.Code)
                        {
                            ConnectionId = command.ConnectionId
                        });
                    }

                    user.FailedPasswordAttempt = 0;
                    await this._userService.UpdateAsync(user, user.Id);

                    List<Claim> claims = new List<Claim>  {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, SaltGenerator.Generate()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("ip", command.Ip)
                };

                    string cacheKey = $"{user.UserName}-{command.Ip}";

                    RefreshTokenModel token;

                    if (this._jwtOptions.ExpiryMinutes != 0)
                    {
                        token = TokenManager.GenerateToken(this._connectionMultiplexer, cacheKey, claims, _jwtOptions.PrivateKey, _jwtOptions.Issuer, _jwtOptions.Audience, this._jwtOptions.ExpiryMinutes);
                    }
                    else
                    {
                        token = TokenManager.GenerateToken(this._connectionMultiplexer, cacheKey, claims, _jwtOptions.PrivateKey, _jwtOptions.Issuer, _jwtOptions.Audience);
                    }

                    await _busClient.PublishAsync(new GetTokenEvent(token)
                    {
                        ConnectionId = command.ConnectionId
                    });
                }

            }
            catch (Exception ex)
            {
                await _busClient.PublishAsync(new GetTokenRejectedEvent(command.UserNameOrEmail, ex.Message, "error")
                {
                    ConnectionId = command.ConnectionId
                });
                throw;
            }

        }
    }
}
