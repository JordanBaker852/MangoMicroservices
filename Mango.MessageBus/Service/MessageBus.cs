using Azure.Messaging.ServiceBus;
using Mango.MessageBus.Service.IService;
using Newtonsoft.Json;
using System.Text;

namespace Mango.MessageBus.Service
{
    public class MessageBus : IMessageBus
    {
        private readonly string _connectionString;

        public MessageBus(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task PublishMessage(object message, string topicQueueName)
        {
            await using var client = new ServiceBusClient(_connectionString, new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            ServiceBusSender sender = client.CreateSender(topicQueueName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage)) 
            { 
                CorrelationId = Guid.NewGuid().ToString()
            };
            
            await sender.SendMessageAsync(finalMessage);

            await sender.DisposeAsync();
            await client.DisposeAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
