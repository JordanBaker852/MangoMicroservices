using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Messaging.IMessaging;
using Mango.Services.EmailAPI.Models.DTO;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AzureServiceBusConsumer> _logger;

        private ServiceBusProcessor _emailCartProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, ILogger<AzureServiceBusConsumer> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _serviceBusConnectionString = _configuration.GetValue<string>("MessageBus:ConnectionString");
            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCart");

            var client = new ServiceBusClient(_serviceBusConnectionString, new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            _emailCartProcessor = client.CreateProcessor(_emailCartQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestRecieved;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
        }

        private async Task OnEmailCartRequestRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDTO cartMessage = JsonConvert.DeserializeObject<CartDTO>(body);

            try
            {
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
