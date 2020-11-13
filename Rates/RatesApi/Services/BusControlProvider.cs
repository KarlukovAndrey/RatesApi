using MassTransit;
using RatesApi.Configuration;

namespace RatesApi
{
    public class BusControlProvider : IBusControlProvider
    {
        private IBusControl _bus;

        public BusControlProvider(ConfigurationProvider configurationProvider)
        {
            var settings = configurationProvider.GetRabbitMqConnectionSettings(); 
            _bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host(settings.Host);
            });
        }

        public void StartBus() => _bus.Start();
        public void StopBus() => _bus.Stop();
        public void PublishMessage<T>(T message) => _bus.Publish(message);
    }
}
