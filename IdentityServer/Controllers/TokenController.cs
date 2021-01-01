using IdentityServer.Attributes;
using IdentityServer.Controllers.objects;
using IdentityServer.Data;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using PARSGREEN.RESTful.SMS.Model.User;
using SharedLibrary;
using System;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly Identity _identity;
        private readonly IEmail _email;

        private ISMS _sms { get; }

        public TokenController(Identity identity, ISMS sms, IEmail email)
        {
            _identity = identity;
            _sms = sms;
            _email = email;
        }


        [Route("login")]
         
        [HttpPost]
        public async Task<GeneralResult<wsLogin.Output>> Login( [FromBody] wsLogin.Input info)
        {

            //_email.SendEmail("nasser.karimi@padraholding.com", "Verification code from Padran Holding!", "HEllO THIS IS MY EMAIL");


            var result = new GeneralResult<wsLogin.Output>();
            do
            {
                var apiKey = Request.Headers["api-key"];
                if (String.IsNullOrEmpty(apiKey))
                {
                    result.SetError("api-key is not provided in header");
                    break;
                }
                _identity.ApiKey = Guid.Parse(apiKey);
                _identity.UserName = info.userName;
                _identity.Password = info.password;
                var apiResult = _identity.Authentication();

                if (apiResult.HasError)
                {
                    result.SetError(apiResult.Message);
                    break;
                }
                if (!_identity.IsValid)
                {
                    result.SetError("Invalid User ID and/or password!!!");
                    break;
                }
                apiResult = _identity.GetToken();
                if (apiResult.HasError)
                {
                    result.SetError(apiResult.Message);
                    break;
                }

                result.Data = new wsLogin.Output
                {
                    token = apiResult.Data.token
                };
            } while (false);
            return result;
        }

        [Route("MobileActivationCode")]
        [HttpPost]
        public async Task<GeneralResult<dynamic>> MobileActivationCode([FromBody] wsMobilActivation.Input info)
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

                _identity.ApiKey = Guid.Parse(apiKey);
                _identity.Mobile= info.mobile;
                
                result = _identity.SetMobileVerificationCode();

                if (result.HasError)
                {
                    break;
                }

                string[] mobiles = { _identity.Mobile };

                _sms.SendMessage(mobiles, $"Your verification Code is ${_identity.VerificationCode}");


                result.Data = new { verificationCode= _identity.VerificationCode };
                

            } while (false);
            return result;
        }

        [Route("tokenByMobile")]
        [HttpPost]
        public async Task<GeneralResult<dynamic>> TokenByMobile([FromBody] wsUserInfo.Input info)
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

                _identity.VerificationCode = info.code;

                result =  _identity.SetTokenByMobileVerificationCode();

                if (result.HasError)
                {
                    break;
                }

            } while (false);
            return result;
        }

        [Route("EmailActivationCode")]
        [HttpPost]
        public async Task<GeneralResult<dynamic>> EmailActivationCode([FromBody] wsUserInfo.Input info)
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

                _identity.Email= info.email;

                result = _identity.SetEmailVerificationCode();

                if (result.HasError)
                {
                    break;
                }

                var emailResult = _email.SendEmail(_identity.Email, "Verification code from Padran Holding", $"Your verification Code is <strong> {_identity.VerificationCode} </strong>");
                


                result.Data = new { verificationCode = _identity.VerificationCode };


            } while (false);
            return result;
        }

        [Route("tokenByEmail")]
        [HttpPost]
        public async Task<GeneralResult<dynamic>> TokenByEmail([FromBody] wsUserInfo.Input info)
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
                _identity.VerificationCode = info.code;

                result = _identity.SetTokenByEmailVerificationCode();

                if (result.HasError)
                {
                    break;
                }

            } while (false);
            return result;
        }
    }

}
