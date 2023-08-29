namespace SelectU.Contracts.Entities
{
    public class Scholarship
    {
        public Guid Id {get; set;}
        public string? School { get; set; }
        public string? ImageURL { get; set; }
        public string? Value1 { get; set; }
        public string? Value2 { get; set; }
        public string? ShortDescription1 { get; set; }
        public string? ShortDescription2 { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
    }
}
