using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Database;

internal sealed class PaymentContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionStatus> TransactionStatuses { get; set; }
    public DbSet<TransactionChangeNotification> TransactionChangeNotifications { get; set; }

    public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.Balance)
                .HasColumnName("balance")
                .IsRequired();

            entity.Property(e => e.Hold)
                .HasColumnName("hold")
                .IsRequired();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.SubjectId)
                .HasColumnName("subject_id")
                .IsRequired();

            entity.Property(e => e.PeerId)
                .HasColumnName("peer_id")
                .IsRequired();

            entity.Property(e => e.Amount)
                .HasColumnName("amount")
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasOne<Account>()
                .WithMany()
                .HasForeignKey(e => e.SubjectId);

            entity.HasOne<Account>()
                .WithMany()
                .HasForeignKey(e => e.PeerId);
            
            entity.HasMany<TransactionStatus>()
                .WithOne()
                .HasForeignKey(e => e.TransactionId);
        });

        modelBuilder.Entity<TransactionStatus>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.TransactionId)
                .HasColumnName("transaction_id")
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<string>(
                    v => v.ToString(),
                    v => (TransactionStatusType)Enum.Parse(typeof(TransactionStatusType), v)
                )
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        });

        modelBuilder.Entity<TransactionChangeNotification>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();
            
            entity.Property(e => e.Payload)
                .HasColumnName("payload")
                .IsRequired();
            
            entity.Property(e => e.IsSent)
                .HasColumnName("is_sent")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        });
    }
}