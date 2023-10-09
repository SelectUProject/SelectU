using SelectU.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Entities
{
    public class UserRating
    {
        public Guid Id { get; set; }
        public string? ApplicantId { get; set; }
        public string? ReviewerId { get; set; }
        public Guid? ScholarshipApplicationId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        [ForeignKey("ApplicantId")]
        public virtual User? User { get; set; }
        [ForeignKey("ReviewerId")]
        public virtual User? Reviewer { get; set; }
        [ForeignKey("ScholarshipApplicationId")]
        public virtual ScholarshipApplication? ScholarshipApplication { get; set; }
        public UserRating() { }
    }
}
