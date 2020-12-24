using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class wsInputApiInfo
    {
        public Guid apiKey { get; set; }
        public string apiName{ get; set; }
    }
    public class wsOutputApiInfo
    {
        public Guid apiKey { get; set; }
    }
}
