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
        public static IBusControl bus;
        public static Rates rates = new Rates("RUB,EUR,JPY");
        static async Task Main()
        {
           bus = Bus.Factory.CreateUsingRabbitMq(config =>
           {
                config.Host("localhost");               
            });
            await bus.StartAsync();

            Timer timer = new Timer(4000);
            timer.Elapsed += rates.GetRates;
            timer.Elapsed += Send;
            timer.AutoReset = true;
            timer.Enabled = true;
            
            Console.ReadLine();
            await bus.StopAsync();
        }

        private static async void Send(object source = null, ElapsedEventArgs e = null)
        {       
            await bus.Publish(new RatesMessage { RatesDictionary = rates.ratesResult.rates });          
            await Task.Run(() => Console.ReadKey());          
        }       
    }
}