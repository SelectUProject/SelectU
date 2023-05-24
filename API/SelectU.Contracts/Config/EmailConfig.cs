namespace SelectU.Contracts.Config
{
    public class EmailConfig
    {
        public string DebugEmail { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
        public string AdminEmail { get; set; } = null!;
        public string ExceptionEmailSubject { get; set; } = null!;
        public string RegistrationEmailSubject { get; set; } = null!;
    }
}
