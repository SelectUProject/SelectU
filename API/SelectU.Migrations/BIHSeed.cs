using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using System.Text.Json;

namespace SelectU.Migrations
{
    public static class BIHSeed
    {

        public static void SeedBaseData(SelectUContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(context, roleManager);
            SeedAdmin(context, userManager);
            SeedReviewer(context, userManager);
            SeedStaff(context, userManager);
            SeedUsers(context, userManager);
            SeedScholarships(context);
            SeedScholarshipApplications(context);
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
            if (!roleManager.RoleExistsAsync(UserRoles.Staff).Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = UserRoles.Staff
                };
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(UserRoles.Reviewer).Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = UserRoles.Reviewer
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
        private static void SeedReviewer(SelectUContext context, UserManager<User> userManager)
        {
            string reviewer = "reviewer@selectu.com";
            CreateUser(userManager, reviewer, UserRoles.Reviewer, "Password1");
        }
        private static void SeedStaff(SelectUContext context, UserManager<User> userManager)
        {
            string staff = "staff@selectu.com";
            CreateUser(userManager, staff, UserRoles.Staff, "Password1");
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
                    UserProfile = new UserProfile()
                };

                var result = userManager.CreateAsync(newUser, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newUser, userRole).Wait();
                }
            }
        }

        private static void SeedScholarships(SelectUContext context)
        {
            User user = context.User.Where(x => x.Email == "darcy@selectu.com").FirstOrDefault();
            List<ScholarshipFormSectionDTO> FormSections = new() {
                new ScholarshipFormSectionDTO
                {
                    Type = ScholarshipFormTypeEnum.String,
                    Name = "Name",
                    Required = true,
                },
                new ScholarshipFormSectionDTO
                {
                    Type = ScholarshipFormTypeEnum.String,
                    Name = "Reason",
                    Required = true,
                },
                new ScholarshipFormSectionDTO
                {
                    Type = ScholarshipFormTypeEnum.Date,
                    Name = "Birthday",
                    Required = true,
                }
            };

            var scholarshipTemplate = JsonSerializer.Serialize(FormSections);


            Scholarship scholarship1 = new Scholarship
            {
                ScholarshipCreatorId = user?.Id,
                ScholarshipFormTemplate = scholarshipTemplate,
                School = "Xavier1",
                Value = "$1000 Dollars",
                ShortDescription = "Tech Scholarship1",
                Description = "test1",
                State = "VIC",
                City = "Australia",
                StartDate = DateTimeOffset.Now,
                EndDate = DateTime.Today.AddDays(4),
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };
            CreateScholarship(context, scholarship1);

            Scholarship scholarship2 = new Scholarship
            {
                ScholarshipCreatorId = user?.Id,
                ScholarshipFormTemplate = scholarshipTemplate,
                School = "Xavier2",
                Value = "$1000 Dollars",
                ShortDescription = "Tech Scholarship2",
                Description = "test2",
                State = "VIC",
                City = "Australia",
                StartDate = DateTimeOffset.Now,
                EndDate = DateTime.Today.AddDays(1),
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };
            CreateScholarship(context, scholarship2);

            Scholarship scholarship3 = new Scholarship
            {
                ScholarshipCreatorId = user?.Id,
                ScholarshipFormTemplate = scholarshipTemplate,
                School = "Xavier3",
                Value = "$1000 Dollars",
                ShortDescription = "Tech Scholarship3",
                Description = "test3",
                State = "VIC",
                City = "Australia",
                StartDate = DateTimeOffset.Now,
                EndDate = DateTime.Today.AddDays(32),
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };
            CreateScholarship(context, scholarship3);
        }

        private static void CreateScholarship(SelectUContext context, Scholarship scholarship)
        {
            if (!context.Scholarships.Any(x => x.Description == scholarship.Description))
            {
                context.Scholarships.Add(scholarship);
                context.SaveChanges();
            }
        }

        private static void SeedScholarshipApplications(SelectUContext context)
        {
            User user = context.User.Where(x => x.Email == "jack@selectu.com").FirstOrDefault();

            Scholarship scholarship = context.Scholarships.Where(x => x.School == "Xavier3").FirstOrDefault();

            List<ScholarshipFormSectionAnswerDTO> FormSectionAnswers1 = new() {
                new ScholarshipFormSectionAnswerDTO
                {
                    Name = "Name",
                    Value = "Jack"
                },
                new ScholarshipFormSectionAnswerDTO
                {
                    Name = "Reason",
                    Value = "Need Money"
                },
                new ScholarshipFormSectionAnswerDTO
                {
                  Name = "Birthday",
                    Value = "01/01/2000",
                }
            };
            List<ScholarshipFormSectionAnswerDTO> FormSectionAnswers2 = new() {
                new ScholarshipFormSectionAnswerDTO
                {
                    Name = "Name",
                    Value = "Jack"
                },
                new ScholarshipFormSectionAnswerDTO
                {
                    Name = "Reason",
                    Value = "Need More Money"
                },
                new ScholarshipFormSectionAnswerDTO
                {
                  Name = "Birthday",
                    Value = "01/01/2000",
                }
            };
            List<ScholarshipFormSectionAnswerDTO> FormSectionAnswers3 = new() {
                new ScholarshipFormSectionAnswerDTO
                {
                    Name = "Name",
                    Value = "Jack"
                },
                new ScholarshipFormSectionAnswerDTO
                {
                    Name = "Reason",
                    Value = "Need More More Money"
                },
                new ScholarshipFormSectionAnswerDTO
                {
                  Name = "Birthday",
                    Value = "01/01/2000",
                }
            };

            var scholarshipTemplateAnswer1 = JsonSerializer.Serialize(FormSectionAnswers1);
            var scholarshipTemplateAnswer2 = JsonSerializer.Serialize(FormSectionAnswers2);
            var scholarshipTemplateAnswer3 = JsonSerializer.Serialize(FormSectionAnswers3);


            ScholarshipApplication scholarshipApplication1 = new ScholarshipApplication
            {
                ScholarshipApplicantId = user?.Id,
                ScholarshipId = scholarship.Id,
                ScholarshipFormAnswer = scholarshipTemplateAnswer1,
                Status = StatusEnum.Pending,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };
            CreateScholarshipApplications(context, scholarshipApplication1);

            ScholarshipApplication scholarshipApplication2 = new ScholarshipApplication
            {
                ScholarshipApplicantId = user?.Id,
                ScholarshipId = scholarship.Id,
                ScholarshipFormAnswer = scholarshipTemplateAnswer2,
                Status = StatusEnum.Pending,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };
            CreateScholarshipApplications(context, scholarshipApplication2);

            ScholarshipApplication scholarshipApplication3 = new ScholarshipApplication
            {
                ScholarshipApplicantId = user?.Id,
                ScholarshipId = scholarship.Id,
                ScholarshipFormAnswer = scholarshipTemplateAnswer3,
                Status = StatusEnum.Pending,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };
            CreateScholarshipApplications(context, scholarshipApplication3);
        }

        private static void CreateScholarshipApplications(SelectUContext context, ScholarshipApplication scholarshipApplication)
        {
            if (!context.ScholarshipApplications.Any(x => x.ScholarshipFormAnswer == scholarshipApplication.ScholarshipFormAnswer))
            {
                context.ScholarshipApplications.Add(scholarshipApplication);
                context.SaveChanges();
            }
        }

    }
}
