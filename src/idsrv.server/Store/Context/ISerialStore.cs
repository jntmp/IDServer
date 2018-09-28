using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idsrv.server.Store.Context
{
    public interface ISerialStore
    {
        Task ToggleAccessAsync(string serialId, bool active);
    }
}
