﻿using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class UserDetailsDTO
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Suburb { get; set; }
        public string? Postcode { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ProfilePicUri { get; set; }
        public string? AboutMe { get; set; }
        public string? Role { get; set; }
        public DateTimeOffset? LoginExpiry { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }

        public UserDetailsDTO() { }

        public UserDetailsDTO(User user)
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
            ProfilePicUri = user.ProfilePicID;
            AboutMe = user.AboutMe;
            LoginExpiry = user.LoginExpiry;
            DateCreated = user.DateCreated;
            DateModified = user.DateModified;
        }
        public UserDetailsDTO(User user, string role)
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
            ProfilePicUri = user.ProfilePicID;
            AboutMe = user.AboutMe;
            LoginExpiry = user.LoginExpiry;
            DateCreated = user.DateCreated;
            DateModified = user.DateModified;
            Role = role;
        }
    }
}
