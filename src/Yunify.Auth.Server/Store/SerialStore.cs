using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Yunify.Auth.Server.Context;
using Yunify.Auth.Server.Store.Context;

namespace Yunify.Auth.Server.Store
{
    public class SerialStore : ISerialStore
    {
        private readonly UserDbContext _context;
        private readonly ILogger _logger;

        public SerialStore(UserDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("SerialStore");
        }

        public async Task ToggleAccessAsync(string serialId, bool active)
        {
            var serial = await _context.Serials.FindAsync(serialId);

            serial.Active = active;

            _context.Serials.Update(serial);

            await _context.SaveChangesAsync();
        }
    }
}
