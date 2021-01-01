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
        public GeneralResult<wsRoleInfo.Output> AddRole([FromBody] wsRoleInfo.Input info)
        {
            var result = new GeneralResult<wsRoleInfo.Output>();
            do
            {
                var apiKey = Request.Headers["api-key"];
                if (String.IsNullOrEmpty(apiKey))
                {
                    result.SetError("api-key is not provided in header");
                    break;
                }
                _role.RoleInfo = new RoleModel()
                {
                    ApiKey = Guid.Parse(apiKey),
                    RoleName = info.roleName
                }; 

                var apiResult = _role.AddRole();
                if (apiResult.HasError)
                {
                    result.SetError(apiResult.Message);
                    break;
                }
                result.Data = new wsRoleInfo.Output()
                {
                    roleId= _role.RoleInfo.RoleId
                };
            } while (false);
            return result;
        }

        [HttpGet]
        public GeneralResult<dynamic> GetAllRoles()
        {
            var result = new GeneralResult<dynamic>();
            do
            {
                var apiKey = Request.Headers["api-key"];
                if (String.IsNullOrEmpty(apiKey))
                {
                    result.SetError("api-key is not provided in header");
                    break;
                }
                _role.RoleInfo = new RoleModel()
                {
                    ApiKey = Guid.Parse(apiKey),
                };

                result = _role.GetAallRoles();
                if (result.HasError)
                {
                    break;
                }
                
            } while (false);
            return result;
        }
    }
    
}