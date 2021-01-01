using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers.objects
{
    public class wsLogin
    {
        public class Input
        {
            public string userName { get; set; }
            public string password{ get; set; }

        }
        public class Output
        {
            public string token { get; set; }
        }
    }
}
