using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using idsrv.api.Config;
using idsrv.api.Models;

namespace idsrv.api.Client
{
    public class AuthClient : IAuthClient
    {
        private readonly IOptions<AuthClientConfig> _authClientConfig;
        private readonly Dictionary<string, UserModel> UserLookup;

        public AuthClientConfig Config { get => _authClientConfig.Value; }

        public AuthClient(IOptions<AuthClientConfig> authClientConfig)
        {
            _authClientConfig = authClientConfig;

            UserLookup = new Dictionary<string, UserModel>
            {
                { "12345", new UserModel("12345", "Alice", "Macy", String.Empty) },
                { "67890", new UserModel("67890", "Bob", "Builder", String.Empty) }
            };
        }

        private async Task<DiscoveryResponse> DiscoverServer()
        {
            var discovery = await DiscoveryClient.GetAsync(_authClientConfig.Value.Authority);

            if (discovery.IsError)
                throw new Exception(discovery.Error);
            else
                return discovery;
        }

        public async Task<TokenResponse> RequestToken(string userName, string password, string scope)
        {
            var discovery = DiscoverServer();

            using (var client = new TokenClient((await discovery).TokenEndpoint, _authClientConfig.Value.ClientId, _authClientConfig.Value.ClientSecret))
            {
                return await client.RequestResourceOwnerPasswordAsync(userName, password, scope);
            }
        }

        public async Task<HttpResponseMessage> ToggleSerialAccess(string accessToken, string serial, bool active)
        {
            string action = active ? "grant" : "revoke";

            string url = $"{_authClientConfig.Value.Authority}/tenant/{action}/{serial}";

            using (var client = new HttpClient())
            {
                client.SetBearerToken(accessToken);

                return await client.PutAsync(url, null);
            }
        }

        public async Task<HttpResponseMessage> DeleteUser(string accessToken, string userId)
        {
            string url = $"{_authClientConfig.Value.Authority}/tenant/delete/{userId}";

            using (var client = new HttpClient())
            {
                client.SetBearerToken(accessToken);

                return await client.DeleteAsync(url);
            }
        }

        public UserModel LookupUserInfo(string userId)
        {
            if (!UserLookup.ContainsKey(userId))
                throw new Exception($"User does not exist with id {userId}");
            else
                return UserLookup[userId];
        }
    }
}
