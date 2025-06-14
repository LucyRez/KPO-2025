using ImageConverter.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageConverter.WebApi.Database;

internal sealed class AppDbContext : DbContext
{
    public DbSet<ConversionTask> ConversionTasks { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConversionTask>(entity =>
        {
            entity.ToTable("conversion_tasks");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.SourceImageId)
                .HasColumnName("source_image_id")
                .IsRequired();

            entity.Property(e => e.ConvertedImageId)
                .HasColumnName("converted_image_id");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<ConversionTaskStatus>(v)
                )
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        });
    }
}