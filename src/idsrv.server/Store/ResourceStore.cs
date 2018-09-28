using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idsrv.server.Context;

namespace idsrv.server.Store
{
    public class ResourceStore : IResourceStore
    {
        private readonly StoreContext _context;
        private readonly ILogger _logger;

        public ResourceStore(StoreContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("ResourceStore");
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var apiResource = await _context.ApiResources.FindAsync(name);

            apiResource.Map();

            return apiResource.ApiResource;
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null)
                throw new ArgumentNullException(nameof(scopeNames));

            var apiResources = new List<ApiResource>();

            var apiResourcesEntities = _context.ApiResources.Where(a => scopeNames.Contains(a.ApiResourceName));

            foreach (var apiResourceEntity in apiResourcesEntities)
            {
                apiResourceEntity.Map();

                apiResources.Add(apiResourceEntity.ApiResource);
            }

            return Task.FromResult(apiResources.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null)
                throw new ArgumentNullException(nameof(scopeNames));

            var identityResources = new List<IdentityResource>();

            var identityResourcesEntities = _context.IdentityResources.Where(i => scopeNames.Contains(i.IdentityResourceName));

            foreach (var identityResourceEntity in identityResourcesEntities)
            {
                identityResourceEntity.Map();

                identityResources.Add(identityResourceEntity.IdentityResource);
            }

            return Task.FromResult(identityResources.AsEnumerable());
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var apiResourcesEntities = _context.ApiResources.ToList();
            var identityResourcesEntities = _context.IdentityResources.ToList();

            var apiResources = new List<ApiResource>();
            var identityResources = new List<IdentityResource>();

            foreach (var apiResourceEntity in apiResourcesEntities)
            {
                apiResourceEntity.Map();

                apiResources.Add(apiResourceEntity.ApiResource);
            }

            foreach (var identityResourceEntity in identityResourcesEntities)
            {
                identityResourceEntity.Map();

                identityResources.Add(identityResourceEntity.IdentityResource);
            }

            var result = new Resources(identityResources, apiResources);

            return Task.FromResult(result);
        }
    }
}
