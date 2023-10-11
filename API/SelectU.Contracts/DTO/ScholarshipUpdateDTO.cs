using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using System.Text.Json;

namespace SelectU.Contracts.DTO
{
    public class ScholarshipUpdateDTO
    {
        public Guid? Id { get; set; }
        public string? School { get; set; }
        public string? ImageURL { get; set; }
        public string? Value { get; set; }
        public string ScholarshipCreatorId { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public List<ScholarshipFormSectionDTO> ScholarshipFormTemplate { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public StatusEnum? Status { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public ScholarshipUpdateDTO() { }

        public ScholarshipUpdateDTO(Scholarship scholarship)
        {
            Id = scholarship.Id;
            School = scholarship.School;
            ScholarshipCreatorId = scholarship.ScholarshipCreatorId;
            ImageURL = scholarship.ImageURL;
            Value = scholarship.Value;
            Status = scholarship.Status;
            ShortDescription = scholarship.ShortDescription;
            Description = scholarship.Description;
            ScholarshipFormTemplate = JsonSerializer.Deserialize<List<ScholarshipFormSectionDTO>>(scholarship.ScholarshipFormTemplate);
            City = scholarship.City;
            State = scholarship.State;
            StartDate = scholarship.StartDate;
            EndDate = scholarship.EndDate;
        }
    }
}