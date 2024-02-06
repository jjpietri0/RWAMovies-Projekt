using Microsoft.EntityFrameworkCore;

namespace IntegrationModule.Models;

public partial class ProjectDBContext : DbContext
{

    private readonly IConfiguration config;
    public ProjectDBContext(IConfiguration configuration)
    {
        config = configuration;
    }

    public ProjectDBContext(DbContextOptions<ProjectDBContext> options, IConfiguration configuration)
        : base(options)
    {
        config = configuration;
    }
    public virtual DbSet<Country> Country { get; set; }

    public virtual DbSet<Genre> Genre { get; set; }

    public virtual DbSet<Image> Image { get; set; }

    public virtual DbSet<Notification> Notification { get; set; }

    public virtual DbSet<Tag> Tag { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Video> Video { get; set; }

    public virtual DbSet<VideoTag> VideoTag { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.Code)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.Body).HasMaxLength(1024);
            entity.Property(e => e.ReceiverEmail).HasMaxLength(256);
            entity.Property(e => e.Subject).HasMaxLength(256);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(256);
            entity.Property(e => e.PwdHash).HasMaxLength(256);
            entity.Property(e => e.PwdSalt).HasMaxLength(256);
            entity.Property(e => e.SecurityToken).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.CountryOfResidence).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryOfResidenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Country");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.ToTable("Video");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.StreamingUrl).HasMaxLength(256);

            entity.HasOne(d => d.Genre).WithMany(p => p.Videos)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Video_Genre");

            entity.HasOne(d => d.Image).WithMany(p => p.Videos)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("FK_Video_Images");
        });

        modelBuilder.Entity<VideoTag>(entity =>
        {
            entity.ToTable("VideoTag");

            entity.HasOne(d => d.Tag).WithMany(p => p.VideoTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoTag_Tag");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoTags)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoTag_Video");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
