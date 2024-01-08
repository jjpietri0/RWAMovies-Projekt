using IntegrationModule.Models;

namespace IntegrationModule.REQModels
{
    public class VideoReq
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int GenreId { get; set; }
        public int? ImageId { get; set; }
        public string StreamingUrl { get; set; }
        public string ImageUrl { get; set; }
        public int TotalSeconds { get; set; }

    }
}
