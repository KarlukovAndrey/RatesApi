using System;
using System.Threading.Tasks;
using RatesApi.Configuration;

namespace RatesApi
{
    class Program
    {        
        static async Task Main()
        {    
            var busControl = DiContainer.GetService<IBusControlProvider>();           
            var timerService = DiContainer.GetService<TimerService>(); 
            busControl.StartBus();

            timerService.SetUpAndStart();
            
            Console.ReadLine();
            busControl.StopBus();
        }           
    }
}