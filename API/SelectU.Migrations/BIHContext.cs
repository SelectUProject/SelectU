using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users")
                .HasOne(e => e.UserProfile)
                .WithOne(e => e.User)
                .HasForeignKey<UserProfile>(e => e.UserId)
                .IsRequired();

            modelBuilder.Entity<UserProfile>().ToTable("UserProfiles")
                .HasMany(e => e.WorkExperience)
                .WithOne(e => e.UserProfile)
                .HasForeignKey(e => e.UserProfileId);

            modelBuilder.Entity<UserProfile>(x =>
            {
                x.HasKey(y => y.UserProfileId);
                x.Property(y => y.Certifications)
                    .HasConversion(
                        from => string.Join(";", from),
                        to => string.IsNullOrEmpty(to) ? new List<string>() : to.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                        new ValueComparer<ICollection<string>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()
                    )
                );
            });
            modelBuilder.Entity<UserProfile>(x =>
            {
                x.HasKey(y => y.UserProfileId);
                x.Property(y => y.Skills)
                    .HasConversion(
                        from => string.Join(";", from),
                        to => string.IsNullOrEmpty(to) ? new List<string>() : to.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                        new ValueComparer<ICollection<string>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()
                    )
                );
            });


            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasKey(k => new { k.RoleId, k.UserId });
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("UserRoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}
