using SelectU.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public string? ReviewerId { get; set; }
        public Guid ScholarshipApplicationId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        [ForeignKey("ReviewerId")]
        public virtual User? Reviewer { get; set; }
        [ForeignKey("ScholarshipApplicationId")]
        public virtual ScholarshipApplication? ScholarshipApplication { get; set; }
    }
}
