using Microsoft.EntityFrameworkCore.Metadata;
using SelectU.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Entities
{
    public class UserProfile
    {
        public UserProfile() { Id = Guid.NewGuid(); }
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid? WorkExperienceId { get; set; }
        public string? ProfilePicID { get; set; }
        public string? AboutMe { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("WorkExperienceId")]
        public virtual ICollection<WorkExperience>? WorkExperience { get; set; }
        public virtual ICollection<string>? Certifications { get; set; }
        public virtual ICollection<string>? Skills { get; set; }
    }
} 
    