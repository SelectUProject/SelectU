using SelectU.Contracts.Enums;
using Newtonsoft.Json.Linq;

namespace SelectU.Contracts.DTO
{
    public class NotificationDTO
    {
        public NotificationTypeEnum NotificationType { get; set; }
        public JObject Data { get; set; }

    }
}
