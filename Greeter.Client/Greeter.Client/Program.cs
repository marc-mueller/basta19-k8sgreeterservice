using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Greeter.Client
{
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Option(Description = "Sets the base URL of the greeter service. Default is http://localhost:2644.", LongName = "service", ShortName = "s")]
        public string ServiceBaseUrl { get; } = "http://localhost:2644";

        [Option(Description = "Sets the polling count. Valid values are: 1-n, 0 = endless polling. Default is 1.", LongName = "polling", ShortName = "p")]
        public int Polling { get; } = 1;

        [Option(Description = "Sets the polling interval in [ms]. Valid values are: 1-n. Default is 1000 [ms].", LongName = "interval", ShortName = "i")]
        public int Interval { get; } = 1000;

        private void OnExecute()
        {
            var greeterService = new GreeterService(new Uri(this.ServiceBaseUrl), this.Polling, this.Interval);

            var cts = new CancellationTokenSource();
            Task greeterTask = greeterService.ShowGreetingsAsync(cts.Token);

            Console.WriteLine("");
            Console.WriteLine("Press enter to exit...");
            Console.WriteLine("");
            Console.ReadLine();

            cts.Cancel();
        }
    }
}