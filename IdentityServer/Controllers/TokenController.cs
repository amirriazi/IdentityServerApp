using IdentityServer.Attributes;
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
        public async Task<GeneralResult> Login( [FromBody] wsInputUserInfo info)
        {

            //_email.SendEmail("nasser.karimi@padraholding.com", "Verification code from Padran Holding!", "HEllO THIS IS MY EMAIL");


            var result = new GeneralResult();
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
                result  = _identity.Authentication();

                if (result.HasError)
                {
                    break;
                }
                if (!_identity.IsValid)
                {
                    result.SetError("Invalid User ID and/or password!!!");
                    break;
                }
                result = _identity.GetToken();
            } while (false);
            return result;
        }

        [Route("MobileActivationCode")]
        [HttpPost]
        public async Task<GeneralResult> MobileActivationCode([FromBody] wsInputUserInfo info)
        {
      
            var result = new GeneralResult();
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
        public async Task<GeneralResult> TokenByMobile([FromBody] wsInputUserInfo info)
        {

            var result = new GeneralResult();
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
        public async Task<GeneralResult> EmailActivationCode([FromBody] wsInputUserInfo info)
        {


            var result = new GeneralResult();
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
        public async Task<GeneralResult> TokenByEmail([FromBody] wsInputUserInfo info)
        {

            var result = new GeneralResult();
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
