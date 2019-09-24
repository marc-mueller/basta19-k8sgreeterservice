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
                Message = "Demo Prep4:" + configuration["ServiceSettings:Message"],
                //Message = "Demo:" + configuration["ServiceSettings:Message"] + $" Prime Number: {FindPrimeNumber(5000)}",
                HostName = Environment.MachineName,
                ServiceVersion = this.GetType().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                TimeStamp = DateTimeOffset.Now
            };

            return Ok(greeting);
        }

        public long FindPrimeNumber(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }
    }
}