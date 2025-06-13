using Microsoft.EntityFrameworkCore;
using SalesAndInventoryDashboard_BE.Models;

namespace SalesAndInventoryDashboard_BE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<SaleItem> SaleItems => Set<SaleItem>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura chave composta: cada produto numa venda é único
            modelBuilder.Entity<SaleItem>()
                .HasKey(si => new { si.SaleId, si.ProductId });

            // Relacionamento: SaleItem pertence a 1 Sale, e Sale tem muitos SaleItems
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId);
        }
    }
}
