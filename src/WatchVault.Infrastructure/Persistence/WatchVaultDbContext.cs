using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public class WatchVaultDbContext : DbContext
{
    public WatchVaultDbContext(DbContextOptions<WatchVaultDbContext> options) : base(options) { }

    public DbSet<Media> Media { get; set; }
    public DbSet<WatchEntry> WatchEntries { get; set; }
    public DbSet<ExternalId> ExternalIds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(500).IsRequired();

            // Store genres as PostgreSQL jsonb - not a join table
            entity.Property(e => e.Genres)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new());
            
            entity.HasMany(e => e.ExternalIds)
                .WithOne(e => e.Media)
                .HasForeignKey(e => e.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WatchEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Rating).IsRequired(false);
            entity.HasOne(e => e.Media)
                .WithMany(e => e.WatchEntries)
                .HasForeignKey(e => e.MediaId);
        });
    }
}