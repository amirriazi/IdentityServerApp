using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class wsUserRole
    {
        public class Input
        {
            public string userName { get; set; }
            public string roleName { get; set; }

        }
        public class Output
        {
            public Guid userId { get; set; }

        }
    }
}
