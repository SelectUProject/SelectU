using SelectU.Contracts.Constants;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class UserInviteDTO
    {
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset? LoginExpiry { get; set; }
    }
}
