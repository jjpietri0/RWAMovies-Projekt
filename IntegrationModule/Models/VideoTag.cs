﻿namespace IntegrationModule.Models
{
    public partial class VideoTag
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int TagId { get; set; }
        public virtual Video Video { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
