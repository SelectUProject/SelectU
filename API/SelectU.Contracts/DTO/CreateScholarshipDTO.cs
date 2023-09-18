﻿using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class CreateScholarshipDTO
    {
        public string? School { get; set; }
        public string? ImageURL { get; set; }
        public string? Value { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public required string ScholarshipFormTemplate { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
