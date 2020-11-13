using RatesApi.Configuration;
using Timer = System.Timers.Timer;
namespace RatesApi
{
    public class TimerService
    {
        private Timer _timer;
        private RatesService _ratesService;
       
        public TimerService(RatesService ratesService, ConfigurationProvider configurationProvider)
        {
            var timerSettings = configurationProvider.GetTimerSettings();
            _timer = new Timer(timerSettings.Interval);
            _ratesService = ratesService;
        }

        public void SetUpAndStart()
        {
            _timer.Elapsed += _ratesService.GetRatesAndSend;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
    }
}
