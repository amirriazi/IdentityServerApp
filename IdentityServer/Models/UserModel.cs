using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public Guid ApiKey { get; set; }
        public string ApiName { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public string  RoleName { get; set; }
    }
}
