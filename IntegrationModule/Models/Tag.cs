namespace IntegrationModule.Models
{
    public partial class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<VideoTag> VideoTags { get; } = new List<VideoTag>();

    }
}
