using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Yunify.Auth.Server.Model;

namespace Yunify.Auth.Server.Store.User
{
    partial class UserStore : IUserClaimStore<UserModel>
    {
        public async Task AddClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var serialClaim = claims.FirstOrDefault(c => c.Type == "serial");

            await _context.UserClaims.AddAsync(new IdentityUserClaim<string>
            {
                ClaimType = "serial",
                ClaimValue = user.Serial,
                UserId = user.UserId
            });
        }

        public Task<IList<Claim>> GetClaimsAsync(UserModel user, CancellationToken cancellationToken)
        {
            var claims = new List<Claim>
            {
                new Claim("serial", user.Serial)
            };

            return Task.FromResult<IList<Claim>>(claims);
        }

        public Task<IList<UserModel>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            Expression<Func<UserModel, bool>> expr = null;

            switch(claim.Type)
            {
                default:
                    expr = u => u.Serial == claim.Value;
                    break;
            }

            var users = _context.Users.Where(expr).ToList();

            return Task.FromResult<IList<UserModel>>(users);
        }

        public Task RemoveClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(UserModel user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
