using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class TempUserDTO
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset? LoginExpiry { get; set; }

        public TempUserDTO(User user)
        {
            Id = user.Id;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            LoginExpiry = user.LoginExpiry;
        }
    }
}
