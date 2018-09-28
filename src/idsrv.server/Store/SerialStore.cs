using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using idsrv.server.Context;
using idsrv.server.Store.Context;

namespace idsrv.server.Store
{
    public class SerialStore : ISerialStore
    {
        private readonly StoreContext _context;
        private readonly ILogger _logger;

        public SerialStore(StoreContext context, ILoggerFactory loggerFactory)
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
