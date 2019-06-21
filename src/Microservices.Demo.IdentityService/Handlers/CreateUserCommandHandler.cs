using Microservices.Demo.Core.Commands;
using Microservices.Demo.Core.Enumerations;
using Microservices.Demo.Core.Exceptions;
using Microservices.Demo.Core.MVC;
using Microservices.Demo.IdentityService.Messaging.Commands;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microservices.Demo.IdentityService.Services;
using RawRabbit;
using System;
using System.Threading.Tasks;

namespace Microservices.Demo.IdentityService.Handlers
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;
        private readonly LocalizationService _localizationService;

        public CreateUserCommandHandler(IBusClient busClient, IUserService userService, LocalizationService localizationService)
        {
            this._busClient = busClient;
            this._userService = userService;
            this._localizationService = localizationService;
        }

        public async Task HandleAsync(CreateUserCommand command)
        {
            try
            {
                bool userNameExists = await this._userService.CheckIfUserNameExists(command.UserName);

                if (userNameExists)
                {
                    StatusCode statusCode = this._localizationService.GetLocalizedStatusCode(ResponseCode.UserNameExists);
                    ApiError apiError = new ApiError(statusCode);

                    await _busClient.PublishAsync(new CreateUserRejectedEvent(command.Email, apiError, statusCode.Code)
                    {
                        ConnectionId = command.ConnectionId
                    });

                    return;
                }

                bool emailExists = await this._userService.CheckIfEmailExists(command.Email);

                if (emailExists)
                {
                    StatusCode statusCode = this._localizationService.GetLocalizedStatusCode(ResponseCode.EmailExists);
                    ApiError apiError = new ApiError(statusCode);

                    await _busClient.PublishAsync(new CreateUserRejectedEvent(command.Email, apiError, statusCode.Code)
                    {
                        ConnectionId = command.ConnectionId
                    });

                    return;
                }

                string verificationCode = await this._userService.AddUserAsync(command.UserName, command.Password, command.Email);

                await this._userService.SaveChangesAsync(Guid.Empty);

                await _busClient.PublishAsync(new UserCreatedEvent(command.Email, verificationCode)
                {
                    ConnectionId = command.ConnectionId
                });

                return;
            }
            catch (BusinessException ex)
            {
                await _busClient.PublishAsync(new CreateUserRejectedEvent(command.Email, ex.Message, ex.Code)
                {
                    ConnectionId = command.ConnectionId
                });
            }
            catch (Exception ex)
            {
                await _busClient.PublishAsync(new CreateUserRejectedEvent(command.Email, ex.Message, "error")
                {
                    ConnectionId = command.ConnectionId
                });
            }
        }
    }
}
