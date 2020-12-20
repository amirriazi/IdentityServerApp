using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    public partial class SqlDatabase
    {
        public QueryResult InsertUser(Guid apiKey, string userName, string password, string email, string mobile)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_InsertNewUser";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@userName", Value = userName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@password", Value = password });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@email", Value = email});
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@mobile", Value = mobile});

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        
        public QueryResult EditUser(Guid userId,  string userName,   string email, string mobile)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_EditUser";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@userId", Value = userId});
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@userName", Value = userName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@email", Value = email });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@mobile", Value = mobile });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public QueryResult  AssignUserRole(string userName , string roleName, Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_AssignUserRole";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@userName", Value = userName});
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@roleName", Value = roleName});

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        
        public QueryResult GetUsers(Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetUsers";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
    }
}
