using Microsoft.Extensions.Configuration;

namespace RatesApi.Configuration
{
    public class ConfigurationProvider
    {
        IConfigurationRoot _configuration;
        public ConfigurationProvider()
        {
            _configuration = new ConfigurationBuilder()             
              .AddJsonFile("AppConfig.json")
              .Build();
        }

        public RatesEndpointSettings GetRatesEndpointSettings() => _configuration.GetSection("RatesEndpointSettings").Get<RatesEndpointSettings>();
        public TimerSettings GetTimerSettings() => _configuration.GetSection("TimerSettings").Get<TimerSettings>();
        public RabbitMqConnectionSettings GetRabbitMqConnectionSettings() => _configuration.GetSection("RabbitMqConnectionSettings").Get<RabbitMqConnectionSettings>();
    }
}
