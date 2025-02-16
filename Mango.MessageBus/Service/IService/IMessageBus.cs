namespace Mango.MessageBus.Service.IService
{
    public interface IMessageBus : IDisposable
    {
        public Task PublishMessage(object message, string topicQueueName);
    }
}
