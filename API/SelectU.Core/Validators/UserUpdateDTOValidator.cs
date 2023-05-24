using FluentValidation;
using SelectU.Contracts.DTO;

namespace SelectU.Core.Validators
{
    public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateDTOValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full Name is required");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("DOB is required");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("Mobile is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not a valid email address");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(x => x.Suburb).NotEmpty().WithMessage("Suburb is required");
            RuleFor(x => x.Postcode).NotEmpty().WithMessage("Postcode is required");
            RuleFor(x => x.State).NotEmpty().WithMessage("State is required");
        }
    }
}
