namespace RatesApi
{
    public interface IBusControlProvider
    {
        void PublishMessage<T>(T message);
        void StartBus();
        void StopBus();
    }
}