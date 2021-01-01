using IdentityServer.Data;
using SharedLibrary;
using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class Users
    {
        private readonly SqlDatabase _sqlDatabase;

        public UserModel UserInfo;

        public Users(SqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        public GeneralResult<dynamic> AddUser()
        {
            var result = new GeneralResult<dynamic>();
            do
            {
                var hashedPassword = Shared.HashPassword(UserInfo.Password);
                var dbResult = _sqlDatabase.InsertUser(UserInfo.ApiKey, UserInfo.UserName, hashedPassword, UserInfo.Email, UserInfo.Mobile);
                if (dbResult.ReturnCode != 1 || dbResult.SPCode != 1)
                {
                    result.SetError(dbResult.Text);
                    break;
                }
                if (dbResult.DataSet.Tables[0].Rows.Count <= 0)
                {
                    result.SetError("User Not Found!");
                    break;
                }
                var DS = Shared.DBNull(dbResult.DataSet);
                var rec = DS.Tables[0].Rows[0];
                UserInfo.UserId = (Guid)rec["userId"];
                result.Message = "User Addition has been sucessful.";
            } while (false);
            return result;
        }
        public GeneralResult<dynamic> EditUser()
        {
            var result = new GeneralResult<dynamic>();
            do
            {
                var dbResult = _sqlDatabase.EditUser(UserInfo.UserId, UserInfo.UserName,  UserInfo.Email, UserInfo.Mobile);
                if (dbResult.ReturnCode != 1 || dbResult.SPCode != 1)
                {
                    result.SetError(dbResult.Text);
                    break;
                }
                if (dbResult.DataSet.Tables[0].Rows.Count <= 0)
                {
                    result.SetError("User Not Found!");
                    break;
                }
                var DS = Shared.DBNull(dbResult.DataSet);
                var rec = DS.Tables[0].Rows[0];
                UserInfo.UserId = (Guid)rec["userId"];
                result.Message = "User Edition has been sucessful.";
            } while (false);
            return result;
        }

        public GeneralResult<dynamic> AssignUserRole()
        {
            var result = new GeneralResult<dynamic>();
            do
            {
                var dbResult = _sqlDatabase.AssignUserRole(UserInfo.UserName, UserInfo.RoleName, UserInfo.ApiKey);
                if (dbResult.ReturnCode != 1 || dbResult.SPCode != 1)
                {
                    result.SetError(dbResult.Text);
                    break;
                }
                if (dbResult.DataSet.Tables[0].Rows.Count <= 0)
                {
                    result.SetError("User Not Found!");
                    break;
                }
                var DS = Shared.DBNull(dbResult.DataSet);
                var rec = DS.Tables[0].Rows[0];
                UserInfo.UserId = (Guid)rec["userId"];
                result.Message = "Assigning the role to this user was sucessful.";
            } while (false);
            return result;

        }

        public GeneralResult<dynamic> GetUsers()
        {
            var result = new GeneralResult<dynamic>();
            do
            {
                var dbResult = _sqlDatabase.GetUsers(UserInfo.ApiKey);
                if (dbResult.ReturnCode != 1 || dbResult.SPCode != 1)
                {
                    result.SetError(dbResult.Text);
                    break;
                }
                if (dbResult.DataSet.Tables[0].Rows.Count <= 0)
                {
                    result.SetError("User Not Found!");
                    break;
                }
                var DS = Shared.DBNull(dbResult.DataSet);
                var List = new List<UserModel>();
                for (var i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    var rec = DS.Tables[0].Rows[i];
                    List.Add(new UserModel
                    {
                        ApiKey= (Guid)rec["apiKey"],
                        ApiName = (string)rec["apiName"],
                        UserId = (Guid)rec["userId"],
                        UserName = (string)rec["userName"],
                        Email = (string)rec["email"],
                        Mobile = (string)rec["mobile"],
                       
                        
                    }); ;
                }
                result.Data = List;
                result.Message = "Assigning the role to this user was sucessful.";
            } while (false);
            return result;

        }
    }

   
}
