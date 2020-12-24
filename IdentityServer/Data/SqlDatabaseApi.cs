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
        public QueryResult InsertApi(string apiName)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_InsertNewApi";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@apiName", Value = apiName});

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        
        public QueryResult EditApi(Guid apiKey ,  string apiName)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_EditApi";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey});
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@apiName", Value = apiName });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public QueryResult  setApiMaster(Guid  apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetApiMaster";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        
        public QueryResult GetApis()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetApis";

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
    }
}
