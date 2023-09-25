using Newtonsoft.Json.Linq;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using System.Text.Json;

namespace SelectU.Contracts.DTO
{
    public class ScholarshipApplicationUpdateDTO
    {
        public Guid? Id { get; set; }
        public  string ScholarshipApplicantId { get; set; }
        public  Guid ScholarshipId { get; set; }
        public List<ScholarshipFormSectionAnswerDTO> ScholarshipFormAnswer { get; set; }
        public StatusEnum Status { get; set; }
        public ScholarshipUpdateDTO? Scholarship { get; set; }

        public ScholarshipApplicationUpdateDTO() { }

        public ScholarshipApplicationUpdateDTO(ScholarshipApplication scholarshipApplication)
        {
            Id = scholarshipApplication.Id;
            ScholarshipApplicantId = scholarshipApplication.ScholarshipApplicantId;
            ScholarshipId = scholarshipApplication.ScholarshipId;
            ScholarshipFormAnswer = JsonSerializer.Deserialize<List<ScholarshipFormSectionAnswerDTO>>(scholarshipApplication.ScholarshipFormAnswer);
            Scholarship = new ScholarshipUpdateDTO(scholarshipApplication.Scholarship);
        }

    }
}
