namespace SelectU.Contracts.Config
{
    public class SmtpConfig
    {
        public string Host { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }
}
