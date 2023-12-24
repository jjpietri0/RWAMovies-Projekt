namespace IntegrationModule.ResponseModels
{
    public partial class VideoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int GenreId { get; set; }
        public int TotalSeconds { get; set; }
        public string? StreamingUrl { get; set; }
        public int? ImageId { get; set; }
        public List<TagResponse>? Tags { get; set; }
    }
}
