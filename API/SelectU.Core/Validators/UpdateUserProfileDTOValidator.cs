using FluentValidation;
using SelectU.Contracts.DTO;

namespace SelectU.Core.Validators
{
    public class UpdateUserProfileDTOValidator : AbstractValidator<UpdateUserProfileDTO>
    {
        public UpdateUserProfileDTOValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
        }
    }
}
