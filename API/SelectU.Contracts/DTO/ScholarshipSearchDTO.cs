using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class ScholarshipSearchDTO
    {
        public Guid? Id { get; set; }
        public string? School { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
        public StatusEnum? Status { get; set; }
        public string? Value { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
