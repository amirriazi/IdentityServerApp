using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;
using Microsoft.AspNetCore.Authorization;
using IdentityServer.Attributes;
using IdentityServer.Services.objects;

namespace IdentityServer.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
        private readonly Apis _api;

        public ApiController(Apis apis)
        {
            _api= apis;
        }

       
        [HttpPost("AddApi")]
        public GeneralResult<wsApiInfo.Output> Add([FromBody] wsApiInfo.Input info)
        {

            var result = new GeneralResult<wsApiInfo.Output>();

            do
            {
                _api.ApiInfo= new ApiModel
                {
                    ApiName= info.apiName
                };
                
                var apiResult = _api.AddApi();
                if (apiResult.HasError)
                {
                    result.SetError(apiResult.Message);
                    break;
                }
                result.Data = new wsApiInfo.Output()
                {
                    apiKey= _api.ApiInfo.ApiKey
                };
            } while (false);
            return result;
        }
        [Route("{id}")]
        [HttpPut]
        public GeneralResult<wsApiInfo.Output> EditApi(string id, [FromBody] wsApiInfo.Input info)
        {
            var result = new GeneralResult<wsApiInfo.Output>();
            do
            {

                if (String.IsNullOrWhiteSpace(id))
                {
                    result.SetError("Api Id should be provided!");
                    break;
                }
                var r = Guid.TryParse(id, out var ApiKey);
                info.apiKey= ApiKey;
                _api.ApiInfo= new ApiModel
                {
                    
                    ApiKey= info.apiKey,
                    ApiName= info.apiName,
                };

                var apiResult = _api.EditApi();
                if (apiResult.HasError)
                {
                    result.SetError(apiResult.Message);
                    break;
                }
                result.Data = new wsApiInfo.Output
                {
                    apiKey= _api.ApiInfo.ApiKey
                };
            } while (false);
            return result;
        }

        [Route("setMaster")]
        [HttpPost]
        public GeneralResult<wsApiInfo.Output> SetMaster([FromBody] wsApiInfo.Input info)
        {
            var result = new GeneralResult<wsApiInfo.Output>();
            do
            {

                _api.ApiInfo = new ApiModel
                {
                    ApiKey = info.apiKey,
                };

                var apiResult = _api.setMasterApi();
                if (apiResult.HasError)
                {
                    result.SetError(apiResult.Message);
                    break;
                }
                result.Data = new wsApiInfo.Output()
                {
                    apiKey= _api.ApiInfo.ApiKey
                };
            } while (false);
            return result;
        }
    }
}