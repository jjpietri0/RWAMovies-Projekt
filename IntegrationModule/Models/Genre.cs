namespace IntegrationModule.Models
{
    public partial class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<Video> Videos { get; } = new List<Video>();
    }
}
