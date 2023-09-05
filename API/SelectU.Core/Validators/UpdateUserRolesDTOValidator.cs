using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts.DTO;

namespace SelectU.Core.Validators
{
    public class UpdateUserRolesDTOValidator : AbstractValidator<UpdateUserRolesDTO>
    {
        public UpdateUserRolesDTOValidator()
        {
            RuleFor(x => x).Must(r => (r.RemoveRoles.IsNullOrEmpty() == false || r.AddRoles.IsNullOrEmpty() == false)).WithMessage("You must at least add or remove one role");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
        }
    }
}
