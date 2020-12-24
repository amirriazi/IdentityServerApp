using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services.objects
{
    public class Api
    {
        public Guid ApiKey { get; set; }
        public bool IsMaster { get; set; }

        public string ApiName { get; set; }

    }
}
