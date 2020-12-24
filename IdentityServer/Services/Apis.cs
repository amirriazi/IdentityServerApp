using IdentityServer.Data;
using IdentityServer.Models;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class Apis
    {
        private readonly SqlDatabase _sqlDatabase;
        public  ApiModel ApiInfo;
        public Apis(SqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        public GeneralResult<dynamic> AddApi()
        {
            var result = new GeneralResult<dynamic>();
            do
            {
                var dbResult = _sqlDatabase.InsertApi(ApiInfo.ApiName);
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
                ApiInfo.ApiKey= (Guid)rec["apiKey"];
                result.Message = "Api Addition has been sucessful.";
            } while (false);
            return result;
        }

        public GeneralResult<dynamic> EditApi()
        {
            var result = new GeneralResult<dynamic>();
            do
            {
                var dbResult = _sqlDatabase.EditApi(ApiInfo.ApiKey, ApiInfo.ApiName);
                if (dbResult.ReturnCode != 1 || dbResult.SPCode != 1)
                {
                    result.SetError(dbResult.Text);
                    break;
                }
                if (dbResult.DataSet.Tables[0].Rows.Count <= 0)
                {
                    result.SetError("Api Not Found!");
                    break;
                }
                var DS = Shared.DBNull(dbResult.DataSet);
                var rec = DS.Tables[0].Rows[0];
                ApiInfo.ApiKey= (Guid)rec["apiKey"];
                result.Message = "Api Edition has been sucessful.";
            } while (false);
            return result;
        }

        public GeneralResult<dynamic> setMasterApi()
        {
            var result = new GeneralResult<dynamic>();
            do
            {
                var dbResult = _sqlDatabase.setApiMaster(ApiInfo.ApiKey);
                if (dbResult.ReturnCode != 1 || dbResult.SPCode != 1)
                {
                    result.SetError(dbResult.Text);
                    break;
                }
                if (dbResult.DataSet.Tables[0].Rows.Count <= 0)
                {
                    result.SetError("Api Not Found!");
                    break;
                }
                result.Message = "Setting Api Master has been sucessful.";
            } while (false);
            return result;
        }

    }
}
