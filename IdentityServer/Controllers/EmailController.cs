using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdentityServer.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IEmail _email;
        public EmailController(IEmail email)
        {
            _email = email;
        }

        [Route("test")]
        [HttpGet]
        public IActionResult test()
        {
            var result = _email.SendEmail("mohammad@amirriazi.com", "Verification code from Padran Holding!", "HEllO THIS IS MY EMAIL");

            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}