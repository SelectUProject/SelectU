using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class ScholarshipApplicationSearchDTO
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? School { get; set; }

    }
}
