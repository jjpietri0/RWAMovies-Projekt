namespace IntegrationModule.Properties
{
    public class SmtpClientSettings
    {
        public string SenderEmail { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }
    }
}
