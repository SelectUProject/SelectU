using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Entities
{
    public class WorkExperience
    {
        public WorkExperience() { Id = Guid.NewGuid(); }
        public Guid Id { get; set; }
        public Guid? UserProfileId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? OnGoing { get; set; }
        [ForeignKey("UserProfileId")]
        public virtual UserProfile? UserProfile { get; set; }
    }
}
