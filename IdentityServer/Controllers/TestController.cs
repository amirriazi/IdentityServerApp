using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedLibrary;

namespace IdentityServer.Controllers
{
    [Route("api/test")]
    [ApiController]

    public class TestController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TestController> _logger;

        public TestController(IConfiguration configuration, ILogger<TestController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [Route("ping")]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("PONG");
        }

        [Route("dbconn")]
        [HttpGet]
        public IActionResult DbConn()
        {
            var sqlConnection = _configuration.GetConnectionString("SQLDB");
            return Ok(sqlConnection);
        }

        [Route("hash/{password}")]
        [HttpGet]
        public IActionResult hash(string password)
        {
            var hash = Shared.HashPassword(password);
            return Ok(hash);
        }

        [Route("logerror")]
        [HttpGet]
        public IActionResult logerror(string password)
        {
            _logger.LogError("this is fatal log!");
            return Ok();
        }
    }
}