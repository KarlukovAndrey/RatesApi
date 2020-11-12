using RatesApi.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Timers;

namespace RatesApi
{
    public class Rates
    {
        string _symbols;
        public Rates(string symbols)
        {
            _symbols = symbols;
        }
        HttpClient client = new HttpClient();
        public  ResponseModel ratesResult;


        public  void GetRates(object source = null, ElapsedEventArgs e = null)
        {
            string baseUrl = "https://openexchangerates.org/api/latest.json";
            string appId = "663f6a0f9c604f439a5fc905ef766f18";           
            string url = baseUrl + "?app_id=" + appId + "&symbols=" + _symbols;
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
                ratesResult = response.Content.ReadAsAsync<ResponseModel>().Result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }
    }
}
