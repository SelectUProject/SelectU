namespace SelectU.Contracts.DTO
{
    public class ValidateUniqueEmailAddressRequestDTO
    {
        public string? Email { get; set; }
    }
    public class ValidateUniqueEmailAddressResponseDTO
    {
        public bool IsUnique { get; set; }
        public bool? IsTempUser { get; set; }
        public string? Message { get; set; }
    }
}
