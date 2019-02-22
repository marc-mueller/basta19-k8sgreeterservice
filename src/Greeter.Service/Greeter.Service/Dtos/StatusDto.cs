namespace Greeter.Service.Dtos
{
    public class StatusDto
    {
        public string MachineName { get; set; }
        public string AssemblyInfoVersion { get; set; }
        public string AssemblyVersion { get; set; }
        public string AssemblyFileVersion { get; set; }
        public string DeploymentEnvironment { get; set; }
    }
}