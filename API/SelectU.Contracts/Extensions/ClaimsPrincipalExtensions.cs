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

            return UserRoleEnum.User;
            //throw new Exception("Unable to retrieve User's role");
        }
    }
}
