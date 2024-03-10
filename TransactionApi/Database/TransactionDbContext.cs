using Microsoft.EntityFrameworkCore;
using TransactionApi.Domain.Model;


namespace TransactionApi.Database;

public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18, 2)");
    }

    public DbSet<Transaction> Transactions { get; set; }
}

