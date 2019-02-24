using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Greeter.Client.Dtos;
using Newtonsoft.Json;

namespace Greeter.Client
{
    public class GreeterService
    {
        private readonly HttpClient client;

        private readonly int polling;

        private readonly int interval;

        public GreeterService(Uri serviceHostBaseUri, int polling, int interval)
        {
            this.client = new HttpClient();
            client.BaseAddress = serviceHostBaseUri ?? throw new ArgumentNullException(nameof(serviceHostBaseUri));
            client.Timeout = new TimeSpan(0, 0, 10);

            this.polling = polling;
            if (this.polling < 0)
            {
                this.polling = 1;
            }

            this.interval = interval;
            if (this.interval <= 0)
            {
                this.interval = 1000;
            }
        }

        public async Task ShowGreetingsAsync(CancellationToken ct)
        {
            await Task.Run(async () =>
             {
                 try
                 {
                     int count = 0;
                     do
                     {
                         Task.Run(async () =>
                         {
                             var greeting = await GetGreetingAsync();
                             ShowGreeting(greeting);
                         });
                         await Task.Delay(interval, ct);
                         count++;
                     } while (!ct.IsCancellationRequested && (polling == 0 || (polling > 0 && count < polling)));
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"An error occurred: {ex.ToString()}");
                 }
             }, ct);
        }

        public async Task<GreetingDto> GetGreetingAsync()
        {
            try
            {
                var json = await client.GetStringAsync("/api/greeter");
                GreetingDto greeting = JsonConvert.DeserializeObject<GreetingDto>(json);
                return greeting;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while reading greeting: {ex.ToString()}");
            }

            return null;
        }

        private object lockobj = new object();
        private void ShowGreeting(GreetingDto greeting)
        {
            lock (lockobj)
            {
                //Console.WriteLine(greeting);

                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{greeting.Message}");
                Console.ForegroundColor = GetForegroundColorForContainer(greeting.HostName);
                Console.Write($" - {greeting.HostName}");
                Console.ForegroundColor = GetForegroundColorForVersion(greeting.ServiceVersion);
                Console.Write($" [{greeting.ServiceVersion}]");
                Console.ResetColor();
                Console.WriteLine($" @ {greeting.TimeStamp.DateTime.ToString() } ");
                Console.ResetColor();
            }
        }

        private ConsoleColor[] predefinedForegroundColors = new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Magenta };

        private Dictionary<string, ConsoleColor> containerColors = new Dictionary<string, ConsoleColor>();
        private ConsoleColor GetForegroundColorForContainer(string hostName)
        {
            if (!containerColors.ContainsKey(hostName))
            {
                containerColors.Add(hostName, predefinedForegroundColors[(containerColors.Count + 1) % predefinedForegroundColors.Length] );
            }

            return containerColors[hostName];
        }

        private Dictionary<string, ConsoleColor> versionColors = new Dictionary<string, ConsoleColor>();
        private ConsoleColor GetForegroundColorForVersion(string version)
        {
            if (!versionColors.ContainsKey(version))
            {
                versionColors.Add(version, predefinedForegroundColors[(versionColors.Count + 1) % predefinedForegroundColors.Length]);
            }

            return versionColors[version];
        }
    }
}