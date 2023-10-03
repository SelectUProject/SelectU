using SelectU.Contracts.Enums;

namespace SelectU.Contracts.DTO
{
    public class TempUserUpdateDTO
    {
        public string? Id { get; set; }
        public DateTimeOffset? LoginExpiry { get; set; }
    }
}
