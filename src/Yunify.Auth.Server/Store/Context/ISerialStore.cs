using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yunify.Auth.Server.Store.Context
{
    public interface ISerialStore
    {
        Task ToggleAccessAsync(string serialId, bool active);
    }
}
