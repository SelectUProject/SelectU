using Microsoft.EntityFrameworkCore.Metadata;
using SelectU.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Entities
{
    public class UserProfile
    {
        public UserProfile() { UserProfileId = Guid.NewGuid().ToString(); }
        public string UserProfileId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; } = null!;
        public string? ProfilePicID { get; set; }
        public string? AboutMe { get; set; }
        public ICollection<WorkExperience> WorkExperience { get; set; } = new List<WorkExperience>();
        public ICollection<string>? Certifications { get; set; }
        public ICollection<string>? Skills { get; set; }
        

    }
    public class WorkExperience
    {
        public WorkExperience() { Id = Guid.NewGuid().ToString(); }
        public string Id { get; set; }
        public UserProfile? UserProfile { get; set; }
        public string? UserProfileId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? OnGoing { get; set; }
    } 
}
