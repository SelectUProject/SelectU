using Microsoft.AspNetCore.Http;
using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class ScholarshipFormSectionAnswerDTO
    {
        public string Name { get; set; } = null!;
        public ScholarshipFormTypeEnum Type { get; set; }
        public string Value { get; set; } = null!;
    }
}
