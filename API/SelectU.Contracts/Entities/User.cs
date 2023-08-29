using Microsoft.AspNetCore.Identity;
using SelectU.Contracts.Enums;


namespace SelectU.Contracts.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? Address { get; set; }
        public string? Suburb { get; set; }
        public string? Postcode { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public required UserProfile UserProfile { get; set; } = null!;
    }
}
