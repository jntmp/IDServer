using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using idsrv.server.Model;
using idsrv.server.Store.Context;

namespace idsrv.server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantController : ControllerBase
    {
        readonly ISerialStore _serialStore;
        readonly IUserStore<UserModel> _userStore;

        public TenantController(ISerialStore serialStore, IUserStore<UserModel> userStore)
        {
            _serialStore = serialStore;
            _userStore = userStore;
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "tokenIdentityServerAuthenticationJwt")]
        [Route("revoke/{serial}")]
        public async Task<ActionResult> Revoke([FromRoute]string serial)
        {
            await _serialStore.ToggleAccessAsync(serial, false);
            
            return await Task.FromResult(Ok());
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "tokenIdentityServerAuthenticationJwt")]
        [Route("grant/{serial}")]
        public async Task<ActionResult> Grant([FromRoute]string serial)
        {
            await _serialStore.ToggleAccessAsync(serial, true);

            return await Task.FromResult(Ok());
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "tokenIdentityServerAuthenticationJwt")]
        [Route("delete/{userId}")]
        public async Task<ActionResult> Delete([FromRoute]string userId)
        {
            var user = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            IdentityResult result = await _userStore.DeleteAsync(user, CancellationToken.None);

            if (result.Succeeded)
                return await Task.FromResult(Ok());
            else
                return await Task.FromResult(new StatusCodeResult(StatusCodes.Status500InternalServerError));
        }
    }
}
