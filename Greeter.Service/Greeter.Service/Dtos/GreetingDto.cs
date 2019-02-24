using System;

namespace Greeter.Service.Dtos
{
    public class GreetingDto
    {
        public string Message { get; set; }
        public string HostName { get; set; }
        public string ServiceVersion { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}