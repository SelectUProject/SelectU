using SelectU.Contracts.DTO;

namespace SelectU.Contracts.Infrastructure
{
    public interface IServiceBusQueueClient
    {
        Task EnqueueNotifications(List<NotificationDTO> prizeUserNotifications);
    }
}