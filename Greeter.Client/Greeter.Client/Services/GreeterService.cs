using System;
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
                         var greeting = await GetGreetingAsync();
                         ShowGreeting(greeting);
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

        private void ShowGreeting(GreetingDto greeting)
        {
            //Console.WriteLine(greeting);

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{greeting.Message}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" - {greeting.HostName}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" [{greeting.ServiceVersion}]");
            Console.ResetColor();
            Console.WriteLine($" @ {greeting.TimeStamp.DateTime.ToString() } ");
            Console.ResetColor();
        }
    }
}