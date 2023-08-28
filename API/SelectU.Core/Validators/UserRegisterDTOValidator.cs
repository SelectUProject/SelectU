using FluentValidation;
using SelectU.Contracts.DTO;

namespace SelectU.Core.Validators
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Mobile is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not a valid email address");
            RuleFor(x => x.State).NotEmpty().WithMessage("State is required");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
