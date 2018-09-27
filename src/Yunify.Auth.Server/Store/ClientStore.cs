using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Yunify.Auth.Server.Context;

namespace Yunify.Auth.Server.Store
{
    public class ClientStore : IClientStore
    {
        private readonly ConfigurationStoreContext _context;
        private readonly ILogger _logger;

        public ClientStore(ConfigurationStoreContext context, ILoggerFactory loggerFactory)
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
