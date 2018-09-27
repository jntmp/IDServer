using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace Yunify.Auth.Api.Controllers
{
    public abstract class BaseAuthController : ControllerBase
    {
        protected string GetUserClaimValue(string type)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == type);

            if (claim == null)
                throw new Exception($"Claim {type} not present");

            return claim.Value;
        }
    }
}
