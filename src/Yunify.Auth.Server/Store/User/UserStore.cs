using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yunify.Auth.Server.Context;
using Yunify.Auth.Server.Model;

namespace Yunify.Auth.Server.Store.User
{
    partial class UserStore
    {
        private readonly UserDbContext _context;
        private readonly ILogger _logger;

        public UserStore(UserDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("UserStore");
        }

        public async Task<IdentityResult> CreateAsync(UserModel user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);

            _context.SaveChanges();

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(UserModel user, CancellationToken cancellationToken)
        {
            _context.Users.Remove(user);

            _context.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<UserModel> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            return Task.FromResult(user);
        }

        public Task<UserModel> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _context.Users.FirstOrDefault(u => 
                GetNormalizedUserNameAsync(u, cancellationToken).Result == normalizedUserName);

            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.Normalize().ToUpper());
        }

        public Task<string> GetUserIdAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId);
        }

        public Task<string> GetUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(UserModel user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName.Normalize();
            
            return Task.FromResult(UInt32.MinValue);
        }
        
        public Task SetUserNameAsync(UserModel user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;

            return Task.FromResult(UInt32.MinValue);
        }

        public Task<IdentityResult> UpdateAsync(UserModel user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);

            _context.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
