using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using idsrv.server.Model;
using IdentityServer4.Validation;

namespace idsrv.server.Store.User
{
    public partial class UserStore : IResourceOwnerPasswordValidator
    {
        //public Task<string> GetPasswordHashAsync(UserModel user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult<string>(user.PasswordHash);
        //}

        //public Task<bool> HasPasswordAsync(UserModel user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(!String.IsNullOrEmpty(user.Password));
        //}

        //public Task SetPasswordHashAsync(UserModel user, string passwordHash, CancellationToken cancellationToken)
        //{
        //    user.PasswordHash = passwordHash;

        //    return Task.FromResult(UInt32.MinValue);
        //}

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _context.Users.FindAsync(context.UserName);

            context.Result.IsError = !user.Password.Equals(context.Password);
        }
    }
}
