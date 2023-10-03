using FluentValidation;
using SelectU.Contracts.DTO;

namespace SelectU.Core.Validators
{
    public class TempUserInviteDTOValidator : AbstractValidator<TempUserInviteDTO>
    {
        public TempUserInviteDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not a valid email address");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required");
            RuleFor(x => x.LoginExpiry).NotEmpty().WithMessage("Login Expiry is required");
        }
    }
}
