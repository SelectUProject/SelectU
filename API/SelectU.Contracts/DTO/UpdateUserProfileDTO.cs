using SelectU.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.DTO
{
    public class UpdateUserProfileDTO
    {
        public string UserId { get; set; }
        public string? ProfilePicID { get; set; }
        public string? AboutMe { get; set; }
        public List<WorkExperienceDTO>? WorkExperience { get; set; }
        public List<string>? Certifications { get; set; }
        public List<string>? Skills { get; set; }
    }

}
