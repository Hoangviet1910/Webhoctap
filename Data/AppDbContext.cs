using Microsoft.EntityFrameworkCore;
using HoangViet.Models;

namespace HoangViet.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<SubLesson> SubLessons => Set<SubLesson>();
    public DbSet<SkillMetric> SkillMetrics => Set<SkillMetric>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Track>(entity =>
        {
            entity.ToTable("Tracks");
            entity.HasKey(t => t.Id);
            entity.HasIndex(t => t.Code).IsUnique();
            entity.HasMany(t => t.Topics)
                  .WithOne(tp => tp.Track)
                  .HasForeignKey(tp => tp.TrackId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.ToTable("Topics");
            entity.HasKey(tp => tp.Id);
            entity.HasMany(tp => tp.SubLessons)
                  .WithOne(s => s.Topic)
                  .HasForeignKey(s => s.TopicId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SubLesson>(entity =>
        {
            entity.ToTable("SubLessons");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.IsCompleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<SkillMetric>(entity =>
        {
            entity.ToTable("SkillMetrics");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.CurrentScore).HasDefaultValue(0);
        });
    }
}
