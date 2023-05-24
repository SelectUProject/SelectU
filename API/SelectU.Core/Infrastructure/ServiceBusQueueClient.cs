using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SelectU.Contracts.Config;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Infrastructure;

namespace SelectU.Core.Infrastructure
{
    public class ServiceBusQueueClient : IServiceBusQueueClient
    {
        private readonly ServiceBusConfig _serviceBusConfig;
        private ServiceBusClient _notificationServiceBusClient;
        private ServiceBusSender _notificationServiceBusSender;

        public ServiceBusQueueClient(IOptions<ServiceBusConfig> serviceBusConfig)
        {
            _serviceBusConfig = serviceBusConfig.Value;
        }
        public async Task EnqueueNotifications(List<NotificationDTO> userNotifications)
        {
            _notificationServiceBusClient = new ServiceBusClient(_serviceBusConfig.NotificationConnectionString);
            _notificationServiceBusSender = _notificationServiceBusClient.CreateSender(_serviceBusConfig.NotificationQueueName);

            // Notifications
            var notificationMessageBatch = await _notificationServiceBusSender.CreateMessageBatchAsync();

            //Enqueue notification
            foreach (var notification in userNotifications)
            {
                //Enqueue notification
                notificationMessageBatch = await CreateServiceBusNotificationMessage(notificationMessageBatch, notification);
            }

            await _notificationServiceBusSender.SendMessagesAsync(notificationMessageBatch);
            notificationMessageBatch.Dispose();
            await _notificationServiceBusSender.DisposeAsync();
            await _notificationServiceBusClient.DisposeAsync();
        }

        private async Task<ServiceBusMessageBatch> CreateServiceBusNotificationMessage(ServiceBusMessageBatch messageBatch, NotificationDTO notificationDto)
        {
            var item = JsonConvert.SerializeObject(notificationDto);

            var message = new ServiceBusMessage($"{item}");
            message.ContentType = "application/json";

            if (!messageBatch.TryAddMessage(message))
            {
                await _notificationServiceBusSender.SendMessagesAsync(messageBatch);
                messageBatch.Dispose();
                messageBatch = await _notificationServiceBusSender.CreateMessageBatchAsync();
                messageBatch.TryAddMessage(message);
            }

            return messageBatch;
        }
    }
}
