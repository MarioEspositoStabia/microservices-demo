using Microservices.Demo.IdentityService.Database.Entity;
using System;
using System.Threading.Tasks;

namespace Microservices.Demo.IdentityService.Services
{
    public interface IUserService
    {
        Task<bool> CheckIfUserNameExists(string userName);

        Task<bool> CheckIfEmailExists(string email);

        Task<string> AddUserAsync(string userName, string password, string email, Guid userId);

        Task<User> GetByUserNameAsync(string userName);

        Task<User> GetByEmailAsync(string email);

        Task<User> UpdateAsync(User user, Guid userId);
    }
}
