namespace SelectU.Contracts.Config
{
    public class ServiceBusConfig
    {
        public string? NotificationPrimaryKey { get; set; }
        public string? NotificationConnectionString { get; set; }
        public string? NotificationQueueName { get; set; }
    }
}
