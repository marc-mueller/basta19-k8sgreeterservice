using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Greeter.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Greeter.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
    public class GreeterController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public GreeterController(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // GET api/greeting
        [HttpGet]
        [ProducesResponseType(typeof(GreetingDto), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<GreetingDto>> Get()
        {
            var greeting = new GreetingDto()
            {
                Message = configuration["ServiceSettings:Message"],
                HostName = Environment.MachineName,
                ServiceVersion = this.GetType().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                TimeStamp = DateTimeOffset.Now
            };

            return Ok(greeting);
        }
    }
}