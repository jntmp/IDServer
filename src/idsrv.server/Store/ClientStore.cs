using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using idsrv.server.Context;

namespace idsrv.server.Store
{
    public class ClientStore : IClientStore
    {
        private readonly StoreContext _context;
        private readonly ILogger _logger;

        public ClientStore(StoreContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("ClientStore");
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);

            client.Map();

            return client.Client;
        }
    }
}
