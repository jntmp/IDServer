using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Yunify.Auth.Api.Config;
using Yunify.Auth.Api.Models;

namespace Yunify.Auth.Api.Client
{
    public interface IAuthClient
    {
        AuthClientConfig Config { get; }

        Task<TokenResponse> RequestToken(string userName, string password, string scope);
        Task<HttpResponseMessage> ToggleSerialAccess(string accessToken, string serial, bool active);
        Task<HttpResponseMessage> DeleteUser(string accessToken, string userId);

        UserModel LookupUserInfo(string userId);
    }
}
