namespace SelectU.Contracts.DTO
{
    public class ScholarshipApplicationCreateDTO
    {
        public required Guid ScholarshipId { get; set; }
        public required string ScholarshipFormAnswer { get; set; }

    }
}
