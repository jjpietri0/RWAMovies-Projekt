namespace IntegrationModule.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public virtual ICollection<Video> Videos { get; } = new List<Video>();

    }
}
