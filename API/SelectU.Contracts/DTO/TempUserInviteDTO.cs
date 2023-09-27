using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class TempUserInviteDTO
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset? Expiry { get; set; }
    }
}
