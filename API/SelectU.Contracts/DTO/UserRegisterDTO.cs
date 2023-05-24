using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class UserRegisterDTO
    {
        public string? FullName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? Suburb { get; set; }
        public int? Postcode { get; set; }
        public string? State { get; set; }
        public int MembershipId { get; set; }
        public string? StripeToken { get; set; }
        public string? Coupon { get; set; }
    }
}
