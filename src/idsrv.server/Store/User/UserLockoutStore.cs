using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using idsrv.server.Model;

namespace idsrv.server.Store.User
{
    public partial class UserStore : IUserLockoutStore<UserModel>
    {
        public Task<int> GetAccessFailedCountAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(UserModel user, CancellationToken cancellationToken)
        {
            var serial = await _context.Serials.FindAsync(user.Serial);
            
            if(serial.Active == user.LockoutEnabled)
            {
                await SetLockoutEnabledAsync(user, !serial.Active, cancellationToken);

                DateTimeOffset? lockoutEnd = user.LockoutEnabled ? DateTimeOffset.Now.AddYears(50) : default(DateTimeOffset);

                await SetLockoutEndDateAsync(user, lockoutEnd, cancellationToken);
            }
            
            return await Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnd);
        }

        public async Task<int> IncrementAccessFailedCountAsync(UserModel user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount++;

            await UpdateAsync(user, cancellationToken);

            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(UserModel user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount =    0;

            await UpdateAsync(user, cancellationToken);
        }

        public async Task SetLockoutEnabledAsync(UserModel user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;

            await UpdateAsync(user, cancellationToken);
        }

        public async Task SetLockoutEndDateAsync(UserModel user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;

            await UpdateAsync(user, cancellationToken);
        }
    }
}
