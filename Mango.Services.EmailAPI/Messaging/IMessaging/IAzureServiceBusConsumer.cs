namespace Mango.Services.EmailAPI.Messaging.IMessaging
{
    public interface IAzureServiceBusConsumer : IDisposable
    {
        Task Start();
        Task Stop();
    }
}
