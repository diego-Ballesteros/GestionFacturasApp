using Facturacion.API.Domain.Entities; 
using Microsoft.EntityFrameworkCore;
namespace Facturacion.API.Infrastructure.Persistence.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; } = default!;
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>(entity =>
        {

            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18,2)");

            entity.HasMany(e => e.Details)
                .WithOne(d => d.Invoice)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {

            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(18,2)");

        });
    }
}
