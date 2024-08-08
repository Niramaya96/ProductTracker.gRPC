using Microsoft.EntityFrameworkCore;
using Prod.Entity;

namespace Prod.DB

{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProductInfo> ProductInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-UNNMCAA;Database=StationeryShop;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductInfo>().HasData(
                new ProductInfo() { Id = 1, Name = "Бумага/упак", Price = 199, Count = 3 },
                new ProductInfo() { Id = 2, Name = "Ножницы/шт", Price = 150, Count = 10 },
                new ProductInfo() { Id = 3, Name = "Карандаши/упак", Price = 100, Count = 7 });
        }
    }
}
