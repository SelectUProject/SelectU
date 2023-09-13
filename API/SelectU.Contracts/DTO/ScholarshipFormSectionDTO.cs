using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class ScholarshipFormSectionDTO
    {
        public ScholarshipFormTypeEnum Type { get; set; }
        public string? Name { get; set; }
        public bool Required { get; set; }
    }
}
