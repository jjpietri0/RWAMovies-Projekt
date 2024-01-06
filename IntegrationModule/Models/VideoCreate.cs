namespace IntegrationModule.Models
{
    public partial class VideoCreate
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public int GenreId { get; set; }
        public string ImageUrl { get; set; }
        public int? ImageId { get; set; }
        public string StreamingUrl { get; set; }
        public int TotalSeconds { get; set; }
    }
}
