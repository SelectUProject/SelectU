using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SelectU.Contracts.Entities;
using System.Reflection.Metadata;

namespace SelectU.Migrations
{
    public class SelectUContext : IdentityDbContext<User>
    {
        public SelectUContext(DbContextOptions<SelectUContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<User> User { get; set; }
        public DbSet<Scholarship> Scholarships { get; set; }
        public DbSet<ScholarshipApplication> ScholarshipApplications { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserProfile>()
                .Property(nameof(UserProfile.Certifications))
                .HasConversion<SemicolonSplitStringConverter, SplitStringComparer>();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserProfile>()
                .Property(nameof(UserProfile.Skills))
                .HasConversion<SemicolonSplitStringConverter, SplitStringComparer>();
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasKey(k => new { k.RoleId, k.UserId });
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("UserRoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
        internal class SplitStringComparer : ValueComparer<ICollection<string>>
        {
            public SplitStringComparer() : base(
                (c1, c2) => new HashSet<string>(c1!).SetEquals(new HashSet<string>(c2!)),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())))
            {
            }
        }

        internal abstract class SplitStringConverter : ValueConverter<ICollection<string>, string>
        {
            protected SplitStringConverter(char delimiter) : base(
                v => string.Join(delimiter.ToString(), v),
                v => v.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries))
            {
            }
        }

        internal class SemicolonSplitStringConverter : SplitStringConverter
        {
            public SemicolonSplitStringConverter() : base(';')
            {
            }
        }
    }
}
