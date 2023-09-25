using SelectU.Contracts.Constants;
using SelectU.Contracts.Enums;
using System.Security.Claims;


namespace SelectU.Contracts.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static UserRoleEnum GetLoggedInUserRole(this ClaimsPrincipal principal)
        {
            if (principal.IsInRole(UserRoles.Admin))
            {
                return UserRoleEnum.Admin;
            }
            else if (principal.IsInRole(UserRoles.Reviewer))
            {
                return UserRoleEnum.Reviewer;
            }
            else if (principal.IsInRole(UserRoles.Staff))
            {
                return UserRoleEnum.Staff;
            }

            return UserRoleEnum.User;
            //throw new Exception("Unable to retrieve User's role");
        }
    }
}
