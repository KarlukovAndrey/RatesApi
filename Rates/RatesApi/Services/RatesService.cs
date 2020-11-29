using Exchanging;
using RatesApi.Configuration;
using RatesApi.Model;
using System;
using System.Net.Http;
using System.Timers;

namespace RatesApi
{
    public class RatesService
    {        
        private  ResponseModel _ratesResult;        
        private string _url;
        private RequestSender _requestSender;
        private IBusControlProvider _bus;        
           
        public RatesService(RequestSender requestSender, IBusControlProvider bus, ConfigurationProvider configurationProvider)
        {           
            _requestSender = requestSender;           
            _bus = bus;           
            var config = configurationProvider.GetRatesEndpointSettings();
            _url = $"{config.BaseUrl}?app_id={config.AppId}&symbols={config.CurrencyCodes}";           
        }


        public  void GetRatesAndSend(object source = null, ElapsedEventArgs e = null)
        {           
            try
            {
                _ratesResult = _requestSender.Get<ResponseModel>(_url);            
                _bus.PublishMessage(new RatesMessage { RatesDictionary = _ratesResult.rates });
                Console.WriteLine("Send");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }           
        }      
    }
}
