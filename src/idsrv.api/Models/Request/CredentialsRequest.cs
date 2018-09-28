using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idsrv.api.Models.Request
{
    public class CredentialsRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
