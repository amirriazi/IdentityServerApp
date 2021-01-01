﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class wsApiInfo
    {
        public class Input
        {
            public Guid apiKey { get; set; }
            public string apiName { get; set; }
        }
        public class Output
        {
            public Guid apiKey { get; set; }
        }
    }
}
