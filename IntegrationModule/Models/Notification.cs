namespace IntegrationModule.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RecieverEmail { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime SentAt { get; set; }
    }
}
