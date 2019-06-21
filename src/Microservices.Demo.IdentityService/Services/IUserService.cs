using System;
using System.Threading.Tasks;

namespace Microservices.Demo.IdentityService.Services
{
    public interface IUserService
    {
        Task<bool> CheckIfUserNameExists(string userName);

        Task<bool> CheckIfEmailExists(string email);

        Task<string> AddUserAsync(string userName, string password, string email);

        Task<int> SaveChangesAsync(Guid userId)
    }
}
