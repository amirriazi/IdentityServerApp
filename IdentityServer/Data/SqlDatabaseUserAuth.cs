using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SharedLibrary;
namespace IdentityServer.Data
{
    public partial class SqlDatabase
    {

        public QueryResult Authenticate(string userName, string password, Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_Authenticate";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey});

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@userName", Value = userName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@password", Value = password });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public QueryResult GetUserRolesByUserId(Guid userId, Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetUserRolsByUserId";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@userId", Value = userId});

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);

        }

        
        public QueryResult ActivationCodeForMobil(string mobile, Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_ActivationCodeForMobile";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@mobile", Value = mobile });
   
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        
        public QueryResult AuthenticateByMobileVerificationCode(string code, Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_AuthenticateByMobileVerificationCode";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@code", Value = code });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        
        public QueryResult ActivationCodeForEmail(string email , Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_ActivationCodeForEmail";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@email", Value = email });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public QueryResult AuthenticateByEmailVerificationCode(string code, Guid apiKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_AuthenticateByEmailVerificationCode";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@apiKey", Value = apiKey });

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@code", Value = code });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


    }
}
