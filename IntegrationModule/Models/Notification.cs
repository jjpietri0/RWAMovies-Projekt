namespace IntegrationModule.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string ReceiverEmail { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
