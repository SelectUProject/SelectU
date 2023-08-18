using Microsoft.AspNetCore.Identity;
using SelectU.Contracts.Constants;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Migrations
{
    public static class BIHSeed
    {

        public static void SeedBaseData(SelectUContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(context, roleManager);
            SeedAdmin(context, userManager);
            SeedUsers(context, userManager);
        }

        private static void SeedRoles(SelectUContext context, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(UserRoles.Admin).Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = UserRoles.Admin
                };
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(UserRoles.User).Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = UserRoles.User
                };
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
        private static void SeedAdmin(SelectUContext context, UserManager<User> userManager)
        {
            string admin = "admin@selectu.com";
            CreateUser(userManager, admin, UserRoles.Admin, "Password1");
        }

        private static void SeedUsers(SelectUContext context, UserManager<User> userManager)
        {
            string user1 = "jack@selectu.com";
            CreateUser(userManager, user1, UserRoles.User, "Password1");

            string user2 = "darcy@selectu.com";
            CreateUser(userManager, user2, UserRoles.User, "Password1");

            string user3 = "daniel@selectu.com";
            CreateUser(userManager, user3, UserRoles.User, "Password1");

            string user4 = "will@selectu.com";
            CreateUser(userManager, user4, UserRoles.User, "Password1");

            string user5 = "ryan@selectu.com";
            CreateUser(userManager, user5, UserRoles.User, "Password1");

            string user6 = "ed@selectu.com";
            CreateUser(userManager, user6, UserRoles.User, "Password1");
        }

        private static void CreateUser(UserManager<User> userManager, string userEmail, string userRole, string password)
        {
            if (userManager.FindByEmailAsync(userEmail).Result == null)
            {
                var newUser = new User
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    Gender = GenderEnum.Male,
                    DateCreated = DateTimeOffset.Now,
                };

                var result = userManager.CreateAsync(newUser, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newUser, userRole).Wait();
                }
            }
        }

    }
}
