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
        public QueryResult InsertRole(string roleName, Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_InsertNewRole";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@roleName", Value = roleName});

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
    }
}
