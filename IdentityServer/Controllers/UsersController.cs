using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using IdentityServer.Attributes;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("AddUser")]
        public GeneralResult<dynamic> AddUser([FromBody] wsUserInfo.Input info)
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
        [HttpPost("addRole")]
        public GeneralResult<wsUserRole.Output> AddRole([FromBody] wsUserRole.Input info)
        {
            var result = new GeneralResult<wsUserRole.Output>();
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

                var apiResult = _users.AssignUserRole();
                if (apiResult.HasError)
                {
                    result.SetError(apiResult.Message);
                    break;
                }
                result.Data = new wsUserRole.Output()
                {
                    userId = _users.UserInfo.UserId
                };
            } while (false);
            return result;

        }
        [HttpPost("{id}")]
        public GeneralResult<dynamic> EditUser(string id, [FromBody] wsUserInfo.Input info)
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

                if (String.IsNullOrWhiteSpace(id))
                {
                    result.SetError("User Id should be provided!");
                    break;
                }
                var userId = Guid.Parse(id);

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

        
        [HttpGet]
        public GeneralResult<dynamic> GetAllUser()
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

                _users.UserInfo = new UserModel()
                {
                    ApiKey = Guid.Parse(apiKey)
                };

                var apiResult = _users.GetUsers();
                if (apiResult.HasError)
                {
                    result.SetError(apiResult.Message);
                    break;
                }
                result.Data = apiResult.Data;
            } while (false);
            return result;
        }
    }
}