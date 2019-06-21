using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.IdentityService.Database.Entity;
using Microservices.Demo.IdentityService.Repositories;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Microservices.Demo.IdentityService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDbContext _dbContext;

        public UserService(IUserRepository userRepository, IDbContext dbContext)
        {
            this._userRepository = userRepository;
            this._dbContext = dbContext;
        }

        public async Task<bool> CheckIfUserNameExists(string userName)
        {
            return await _userRepository.AnyAsync(user => user.UserName.ToUpperInvariant() == userName.ToUpperInvariant());
        }

        public async Task<bool> CheckIfEmailExists(string email)
        {
            return await _userRepository.AnyAsync(user => user.Email.ToUpperInvariant() == email.ToUpperInvariant());
        }

        public async Task<string> AddUserAsync(string userName, string password, string email)
        {
            User user = new User(userName, password, email)
            {
                Id = Guid.NewGuid(),
                VerificationCode = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)
            };

            user = await this._userRepository.AddAsync(user);

            return user.VerificationCode;
        }

        public async Task<int> SaveChangesAsync(Guid userId)
        {
            return await this._dbContext.SaveChangesAsync(userId);
        }
    }
}
