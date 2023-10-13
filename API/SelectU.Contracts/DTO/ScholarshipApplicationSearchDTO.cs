using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class ScholarshipApplicationSearchDTO
    {
        public Guid? Id { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public string? School { get; set; }

    }
}
