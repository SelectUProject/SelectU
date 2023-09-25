namespace SelectU.Contracts.DTO
{
    public class ScholarshipApplicationCreateDTO
    {
        public required Guid ScholarshipId { get; set; }
        public required List<ScholarshipFormSectionAnswerDTO> ScholarshipFormAnswer { get; set; }

    }
}
