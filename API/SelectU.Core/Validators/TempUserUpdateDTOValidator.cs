using FluentValidation;
using SelectU.Contracts.DTO;

namespace SelectU.Core.Validators
{
    public class TempUserUpdateDTOValidator : AbstractValidator<TempUserUpdateDTO>
    {
        public TempUserUpdateDTOValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required");
            RuleFor(x => x.LoginExpiry).NotEmpty().WithMessage("Login Expiry is required");
        }
    }
}
