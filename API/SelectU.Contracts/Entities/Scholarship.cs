using System.ComponentModel.DataAnnotations.Schema;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.Entities
{
    public class Scholarship
    {
        public Guid Id {get; set;}
        public required string ScholarshipCreatorId { get; set;}
        public string? School { get; set; }
        public string? ImageURL { get; set; }
        public string? Value { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public required string ScholarshipFormTemplate { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public ScholarshipStatusEnum Status { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }

        [ForeignKey("ScholarshipCreatorId")]
        public virtual User? ScholarshipCreator { get; set; }
        public virtual List<ScholarshipApplication>? ScholarshipApplications { get; set; }

    }
}