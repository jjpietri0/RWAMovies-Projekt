namespace IntegrationModule.ResponseModels
{
    public partial class TagResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<VideoTagResponse> VideoTags { get; } = new List<VideoTagResponse>();
    }
}
