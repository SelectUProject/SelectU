﻿using SelectU.Contracts.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectU.Contracts.Entities
{
    public class ScholarshipApplication
    {
        public Guid Id {get; set;}
        public required string ScholarshipApplicantId { get; set; }
        public required Guid ScholarshipId { get; set; }
        public required string ScholarshipFormAnswer { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public StatusEnum Status { get; set; }

        [ForeignKey("ScholarshipApplicantId")]
        public virtual User? ScholarshipApplicant { get; set; }

        [ForeignKey("ScholarshipId")]
        public virtual Scholarship? Scholarship { get; set; }
    }
}