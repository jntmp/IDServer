using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using idsrv.api.Client;
using idsrv.api.Models;
using idsrv.api.Models.Request;

namespace idsrv.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseAuthController
    {
        readonly IAuthClient _authClient;
        readonly IMemoryCache _cache;

        private const string ACCESS_TOKEN_KEY = "access_token";

        public AuthController(IAuthClient authClient, IMemoryCache cache)
        {
            _authClient = authClient;
            _cache = cache;
        }

        /// <summary>
        /// Tokens will be requested from the identity server, by username and password.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("token")]
        public async Task<ActionResult<TokenResponse>> Token([FromBody]CredentialsRequest credentials)
        {
            var tokenResponse = await _authClient.RequestToken(credentials.UserName, credentials.Password, _authClient.Config.DefaultScope);

            if (!tokenResponse.IsError)
                HttpContext.Response.Cookies.Append(ACCESS_TOKEN_KEY, tokenResponse.AccessToken);

            return tokenResponse;
        }

        /// <summary>
        /// User lookup will be matched by user id to return Api level user meta data (firstName, lastName)
        /// Result will be memory cached.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user")]
        [Authorize]
        public ActionResult<UserModel> UserInfo()
        {
            string subject = base.GetUserClaimValue(JwtClaimTypes.Subject);

            if (!_cache.TryGetValue(subject, out UserModel user))
            {
                user = _authClient.LookupUserInfo(subject);
                user.Serial = base.GetUserClaimValue("serial");

                _cache.Set(subject, user);
            }
            
            return user;
        }

        /// <summary>
        /// Revoke a serial's access        
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("revoke/{serial}")]
        [Authorize]
        public async Task<ActionResult> RevokeSerial([FromRoute]string serial, bool active = false)
        {
            string token = await HttpContext.GetTokenAsync(ACCESS_TOKEN_KEY);

            var response = await _authClient.ToggleSerialAccess(token, serial, active);

            if (!response.IsSuccessStatusCode)
                return new StatusCodeResult((int)response.StatusCode);
            else
                return Ok();
        }

        /// <summary>
        /// Grant a serial's access
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("grant/{serial}")]
        [Authorize]
        public async Task<ActionResult> GrantSerial([FromRoute]string serial)
        {
            return await RevokeSerial(serial, true);
        }

        /// <summary>
        /// Logout the current user by clearing cookies.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(ACCESS_TOKEN_KEY);

            return Ok();
        }

        /// <summary>
        /// Dekete a specific user from the identity server.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{userId}")]
        [Authorize]
        public async Task<ActionResult> Delete([FromRoute]string userId)
        {
            string token = await HttpContext.GetTokenAsync(ACCESS_TOKEN_KEY);

            var response = await _authClient.DeleteUser(token, userId);

            if (!response.IsSuccessStatusCode)
                return new StatusCodeResult((int)response.StatusCode);
            else
                return Ok();
        }
    }
}
