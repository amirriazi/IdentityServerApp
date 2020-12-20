using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PARSGREEN.RESTful.SMS.Model.Message;
using SharedLibrary;

namespace IdentityServer.Services
{
    public class Identity 
    {



        private IConfiguration _configuration { get; }
        private SqlDatabase _sqlDatabase { get; }
        public Identity(IConfiguration configuration, SqlDatabase sqlDatabase)

        {
            _configuration = configuration;
            _sqlDatabase = sqlDatabase;
        }

        public bool IsValid { get; set; }
        public Guid ApiKey { get; set; }
        public Guid UserId { get; set; }
        public string UserName{ get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public List<UserRole> UserRoleList { get; set; }
        


        private string generateToken()
        {
            var result = new GeneralResult();
            do
            {
                result = SetUserRoles();
                if (result.HasError)
                {
                    return result.Message;
                    break;
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(UserId)),
                    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(10)).ToUnixTimeSeconds().ToString()),
                    new Claim("Application", "ir.simpay.Simsell"),
                };

                UserRoleList.ForEach(role =>
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                });


                var token = new JwtSecurityToken(
                                new JwtHeader(
                                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"))),
                                    SecurityAlgorithms.HmacSha256)
                                ),
                                new JwtPayload(claims)
                            );

                var AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                return AccessToken;
            } while (false);
            return "";
        }

        public GeneralResult Authentication()
        {
            var result = new GeneralResult();
            do
            {
                var dbResult = _sqlDatabase.Authenticate(UserName, Shared.HashPassword(Password), ApiKey);
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
                IsValid = (bool)rec["isValid"];
                UserId = (Guid)rec["userId"];
                result.Message = "User Authentication has been sucessful.";
            } while (false);
            return result;
        }
        public GeneralResult GetToken()
        {
            var result = new GeneralResult();
            do
            {
                var token = generateToken();
                result.Data = new { token };
            } while (false);
            return result;
        }


        private GeneralResult SetUserRoles()
        {
            var result = new GeneralResult();
            do
            {
                var dbResult = _sqlDatabase.GetUserRolesByUserId(UserId, ApiKey);
                if (dbResult.ReturnCode != 1 || dbResult.SPCode != 1)
                {
                    result.SetError(dbResult.Text);
                    break;
                }
                if (dbResult.DataSet.Tables[0].Rows.Count <= 0)
                {
                    result.SetError("User Has No Roles!!");
                    break;
                }
                var DS = Shared.DBNull(dbResult.DataSet);
                var List = new List<UserRole>();
                for (var i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    var rec = DS.Tables[0].Rows[i];
                    List.Add(new UserRole
                    {
                        RoleId = (Guid)rec["roleId"],
                        RoleName = (string)rec["roleName"]
                    });
                }
                UserRoleList = List;
            } while (false);
            return result;
        }

        public GeneralResult SetMobileVerificationCode()
        {
            var result = new GeneralResult();
            do
            {
                var dbResult = _sqlDatabase.ActivationCodeForMobil(Mobile, ApiKey);
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
                VerificationCode= (string)rec["code"];

               

                result.Message = "Verification Code has been Created sucessfully.";

            } while (false);
            return result;
        }


        public GeneralResult SetTokenByMobileVerificationCode()
        {
            var result = new GeneralResult();
            do
            {
                var dbResult = _sqlDatabase.AuthenticateByMobileVerificationCode(VerificationCode, ApiKey);
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
                IsValid = (bool)rec["isValid"];
                UserId= (Guid)rec["userId"];
                UserName = (string)rec["userName"];

                if (!IsValid)
                {
                    result.SetError("Code has not been verified!");
                    break;
                }

                var token = generateToken();
                result.Data = new { token };

            } while (false);
            return result;
        }

        public GeneralResult SetEmailVerificationCode()
        {
            var result = new GeneralResult();
            do
            {
                var dbResult = _sqlDatabase.ActivationCodeForEmail(Email, ApiKey);
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
                VerificationCode = (string)rec["code"];
                result.Message = "Verification Code has been Created sucessfully.";

            } while (false);
            return result;
        }

        public GeneralResult SetTokenByEmailVerificationCode()
        {
            var result = new GeneralResult();
            do
            {
                var dbResult = _sqlDatabase.AuthenticateByEmailVerificationCode(VerificationCode, ApiKey);
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
                IsValid = (bool)rec["isValid"];
                UserId = (Guid)rec["userId"];
                UserName = (string)rec["userName"];

                if (!IsValid)
                {
                    result.SetError("Code has not been verified!");
                    break;
                }

                var token = generateToken();
                result.Data = new { token };

            } while (false);
            return result;
        }
    }
}
