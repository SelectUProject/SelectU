using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class UserUpdateDTO
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? Suburb { get; set; }
        public int? Postcode { get; set; }
        public string? State { get; set; }

        public UserUpdateDTO() { }

        public UserUpdateDTO(User user)
        {
            Id = user.Id;
            FullName = user.FullName;
            DateOfBirth = user.DateOfBirth;
            Gender = user.Gender;
            Mobile = user.Mobile;
            Email = user.Email;
            Address = user.Address;
            Suburb = user.Suburb;
            Postcode = user.Postcode;
            State = user.State;
        }
    }
}
