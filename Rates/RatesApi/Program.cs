using System;
using System.IO;
using System.Net.Http;
using System.Timers;

namespace RatesApi
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static void Main()
        {

            Timer timer = new Timer(2000);
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

                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadAsStringAsync().Result;

                string writePath = @"C:\work\С#\RatesApi\rates.txt";

                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(responseBody);
                }


                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }
    }
}