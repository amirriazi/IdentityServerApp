using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly Users _users;

        public UsersController(Users users)
        {
            _users= users;
        }

        [Route("AddUser")]
        [HttpPost]
        public GeneralResult AddUser([FromBody] wsInputUserInfo info)
        {

            var result = new GeneralResult();

            do
            {
                var apiKey = Request.Headers["api-key"];
                if (String.IsNullOrEmpty(apiKey))
                {
                    result.SetError("api-key is not provided in header");
                    break;
                }

                _users.UserInfo = new UserModel
                {
                    ApiKey = Guid.Parse(apiKey),
                    UserName = info.userName,
                    Password = info.password,
                    Email = info.email,
                    Mobile = info.mobile
                };
                
                result = _users.AddUser();
                if (result.HasError)
                {
                    break;
                }
                result.Data = new
                {
                    userId = _users.UserInfo.UserId
                };
            } while (false);
            return result;
        }
        [Route("addRole")]
        [HttpPost]
        public GeneralResult AddRole([FromBody] wsInputUserRole info)
        {
            var result = new GeneralResult();
            do
            {
                var apiKey = Request.Headers["api-key"];
                if (String.IsNullOrEmpty(apiKey))
                {
                    result.SetError("api-key is not provided in header");
                    break;
                }

                if (String.IsNullOrWhiteSpace(info.userName) || String.IsNullOrWhiteSpace(info.roleName)) 
                {
                    result.SetError("User and Role Name should be provided!");
                    break;
                }
                _users.UserInfo = new UserModel
                {
                    ApiKey = Guid.Parse(apiKey),
                    UserName = info.userName,
                    RoleName = info.roleName
                };

                result = _users.AssignUserRole();
                if (result.HasError)
                {
                    break;
                }
                result.Data = new
                {
                    userId = _users.UserInfo.UserId
                };
            } while (false);
            return result;

        }
        [Route("{id}")]
        [HttpPut]
        public GeneralResult EditUser(string id, [FromBody] wsInputUserInfo info)
        {
            var result = new GeneralResult();
            do
            {
                var apiKey = Request.Headers["api-key"];
                if (String.IsNullOrEmpty(apiKey))
                {
                    result.SetError("api-key is not provided in header");
                    break;
                }

                if (String.IsNullOrWhiteSpace(id))
                {
                    result.SetError("User Id should be provided!");
                    break;
                }
                var r = Guid.TryParse(id, out var userId);
                info.userId = userId;
                _users.UserInfo = new UserModel
                {
                    
                    UserId = info.userId,
                    UserName = info.userName,
                    Email = info.email,
                    Mobile = info.mobile
                };

                result = _users.EditUser();
                if (result.HasError)
                {
                    break;
                }
                result.Data = new
                {
                    userId = _users.UserInfo.UserId
                };
            } while (false);
            return result;
        }
    }
}