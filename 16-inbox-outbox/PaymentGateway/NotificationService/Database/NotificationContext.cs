using Microsoft.EntityFrameworkCore;
using NotificationService.Models;

namespace NotificationService.Database;

internal sealed class NotificationContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }

    public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.NotificationKey)
                .HasColumnName("notification_key")
                .IsRequired();
            
            entity.Property(e => e.Payload)
                .HasColumnName("payload")
                .IsRequired();
            
            entity.Property(e => e.IsProcessed)
                .HasColumnName("is_processed")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.NotificationKey)
                .IsUnique();
        });
    }
}