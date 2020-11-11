using MassTransit;
using RatesApi.Model;
using Exchanging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace RatesApi
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host("localhost");

                config.ReceiveEndpoint("test_queue", ep =>
                {
                    ep.Handler<TestMessage>(context =>
                    {
                        Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
                        return Console.Out.WriteLineAsync($"Received: {context.Message.Text2}");
                    });
                });
            });

            await bus.StartAsync(); 
            Console.WriteLine("Publishing message");
            Thread.Sleep(5000);

            await bus.Publish(new TestMessage { Text = "Hi", Text2 = "Go to hell" });

            Console.WriteLine("Press any key to exit");
            await Task.Run(() => Console.ReadKey());

            await bus.StopAsync();

            Timer timer = new Timer(4000);
            timer.Elapsed += GetRates;
            timer.AutoReset = true;
            timer.Enabled = true;
            Console.ReadLine();
        }

        public static void GetRates(object source = null, ElapsedEventArgs e = null)
        {
            string baseUrl = "https://openexchangerates.org/api/latest.json";
            string appId = "663f6a0f9c604f439a5fc905ef766f18";
            string symbols = "RUB,EUR,JPY";
            string url = baseUrl + "?app_id=" + appId + "&symbols=" + symbols;
            try
            {

                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().Result;

                string writePath = @"rates.txt";

                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(responseString);
                }
                Console.WriteLine(responseString);

                var responseResult = response.Content.ReadAsAsync<ResponseModel>().Result;

                Dictionary<string, decimal> ratesDictionary = new Dictionary<string, decimal>
                {
                    ["USDRUB"] = responseResult.rates.RUB,
                    ["USDEUR"] = responseResult.rates.EUR,
                    ["USDJPY"] = responseResult.rates.JPY
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }
    }
}