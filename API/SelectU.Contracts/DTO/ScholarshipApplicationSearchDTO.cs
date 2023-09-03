using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class ScholarshipApplicationSearchDTO
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? School { get; set; }

    }
}
