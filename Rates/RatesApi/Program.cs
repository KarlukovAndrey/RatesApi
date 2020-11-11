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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RatesApi
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        public static Dictionary<string, decimal> ratesResult = new Dictionary<string, decimal>();       
        public static IBusControl bus;
        static async Task Main()
        {
           bus = Bus.Factory.CreateUsingRabbitMq(config =>
           {
                config.Host("localhost");               
            });
            await bus.StartAsync();

            Timer timer = new Timer(4000);
            timer.Elapsed += GetRates;
            timer.Elapsed += Send;
            timer.AutoReset = true;
            timer.Enabled = true;
            
            Console.ReadLine();
            await bus.StopAsync();
        }

        private static async void Send(object source = null, ElapsedEventArgs e = null)
        {       
            await bus.Publish(new TestMessage { RatesDictionary = ratesResult });          
            await Task.Run(() => Console.ReadKey());          
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
                //Console.WriteLine(responseString);
              
                var responseResult = response.Content.ReadAsAsync<ResponseModel>().Result;                

                ratesResult = new Dictionary<string, decimal>
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