using FluentValidation;
using SelectU.Contracts.DTO;

namespace SelectU.Core.Validators
{
    public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordDTOValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Old password is required");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required");
        }
    }
}
