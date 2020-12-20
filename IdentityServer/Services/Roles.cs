using IdentityServer.Data;
using SharedLibrary;
using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class Roles
    {
        private readonly SqlDatabase _sqlDatabase;

        public RoleModel RoleInfo;

        public Roles(SqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        public GeneralResult AddRole()
        {
            var result = new GeneralResult();
            do
            {
                var dbResult = _sqlDatabase.InsertRole(RoleInfo.RoleName, RoleInfo.ApiKey);
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
                RoleInfo.RoleId = (Guid)rec["roleId"];
                result.Message = "User Addition has been sucessful.";
            } while (false);
            return result;
        }
    }

   
}
