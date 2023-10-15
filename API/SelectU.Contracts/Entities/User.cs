using Microsoft.AspNetCore.Identity;
using SelectU.Contracts.DTO;
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
        public string? ProfilePicID { get; set; }
        public string? AboutMe { get; set; }
        public DateTimeOffset? LoginExpiry { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public User() { }
        public User(UserDetailsDTO user)
        {
            Id = user.Id;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            DateOfBirth = user.DateOfBirth;
            Gender = user.Gender;
            PhoneNumber = user.PhoneNumber;
            Address = user.Address;
            Suburb = user.Suburb;
            Postcode = user.Postcode;
            State = user.State;
            Country = user.Country;
            ProfilePicID = user.ProfilePicUri;
            AboutMe = user.AboutMe;
            LoginExpiry = user.LoginExpiry;
            DateCreated = user.DateCreated;
            DateModified = user.DateModified;
        }

    }
}
