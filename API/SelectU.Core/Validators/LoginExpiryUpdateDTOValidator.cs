using FluentValidation;
using SelectU.Contracts.DTO;

namespace SelectU.Core.Validators
{
    public class LoginExpiryUpdateDTOValidator : AbstractValidator<LoginExpiryUpdateDTO>
    {
        public LoginExpiryUpdateDTOValidator()
        {
            RuleFor(x => x.LoginExpiry).NotEmpty().WithMessage("Login Expiry is required");
        }
    }
}
