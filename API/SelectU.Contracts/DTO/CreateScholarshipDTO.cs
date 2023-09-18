using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class CreateScholarshipDTO
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
    }
}
