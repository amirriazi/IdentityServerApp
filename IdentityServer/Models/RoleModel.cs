using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class RoleModel
    {
        public Guid RoleId { get; set; }
        public Guid ApiKey { get; set; }
        public string RoleName { get; set; }
    }
}
