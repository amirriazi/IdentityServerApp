using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        private readonly Roles _role;

        public RolesController(Roles role)
        {
            _role = role;
        }
        [Route("AddRole")]
        [HttpPost]
        public GeneralResult AddRole([FromBody] wsInputRoleInfo info)
        {
            var result = new GeneralResult();
            do
            {
                _role.RoleInfo = new RoleModel()
                {
                    RoleName = info.roleName
                }; 

                result = _role.AddRole();
                if (result.HasError)
                {
                    break;
                }
                result.Data = new
                {
                    roleId= _role.RoleInfo.RoleId
                };
            } while (false);
            return result;
        }
    }
    
}