using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class wsInputUserInfo
    {
        public Guid userId { get; set; }
        public string userName { get; set; }

        public string password { get; set; }
        public string  mobile { get; set; }
        public string email { get; set; }
        public string code { get; set; }
    }
}
