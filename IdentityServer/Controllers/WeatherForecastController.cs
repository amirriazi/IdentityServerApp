using IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [Authorize(Roles ="Admin")]
        [ClaimRequirement("Application", "ir.simpay.Simsell")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

            var tokenStr = authHeader.Substring("Bearer ".Length).Trim();
            System.Console.WriteLine(tokenStr);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(tokenStr) as JwtSecurityToken;
            System.Console.WriteLine(token);
            var payload = token.Claims;


            var application = payload.First(c => c.Type == "Application").Value;

            //var nameid = token.Claims.First(claim => claim.Type == ClaimsPrincipal).Value;

            //var identity = new ClaimsIdentity(token.Claims);
            //var User = new ClaimsPrincipal(identity);


            //System.Console.WriteLine(User);


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
