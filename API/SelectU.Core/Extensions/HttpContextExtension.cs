using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SelectU.Core.Extensions
{
    public static class HttpContextExtension
    {
        public static string GetUserName(this HttpContext context) =>
            context.User.Identity?.Name ?? throw new Exception("Unable to retrieve Username");

        public static string GetUserId(this HttpContext context) =>
            context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Unable to retrieve UserId");
    }
}
