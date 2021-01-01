using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class wsRoleInfo
    {
        public class Input
        {
            public string roleName { get; set; }
        }
        public class Output
        {
            public Guid roleId { get; set; }
        }
    }
}
