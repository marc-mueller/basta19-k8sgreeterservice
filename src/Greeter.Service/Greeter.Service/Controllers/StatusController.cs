using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Greeter.Service.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Greeter.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StatusController : ControllerBase
    {
        private readonly IHostingEnvironment _environment;

        public StatusController(IHostingEnvironment environment)
        {
            this._environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpGet()]
        [ProducesResponseType(typeof(StatusDto), (int)HttpStatusCode.OK)]
        public Task<IActionResult> GetStatus()
        {
            var status = new StatusDto();
            status.AssemblyInfoVersion = this.GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            status.AssemblyVersion = this.GetType().Assembly.GetName().Version.ToString();
            status.AssemblyFileVersion = this.GetType().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            status.MachineName = Environment.MachineName;
            status.DeploymentEnvironment = this._environment?.EnvironmentName ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return Task.FromResult<IActionResult>(Ok(status));
        }
    }
}