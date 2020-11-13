using Microsoft.Extensions.DependencyInjection;

namespace RatesApi.Configuration
{
    public static class DiContainer
    {
        static ServiceProvider _serviceProvider;
       static DiContainer()
       {
            _serviceProvider = new ServiceCollection()
                .AddSingleton<IBusControlProvider, BusControlProvider>()
                .AddSingleton<RequestSender>()
                .AddSingleton<RatesService>()
                .AddSingleton<TimerService>()
                .AddSingleton<ConfigurationProvider>()
                .BuildServiceProvider();         
        }

        public static T GetService<T>() => _serviceProvider.GetService<T>();            
    }
}
