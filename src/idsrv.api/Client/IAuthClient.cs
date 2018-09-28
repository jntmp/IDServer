using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
using idsrv.api.Config;
using idsrv.api.Models;

namespace idsrv.api.Client
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
