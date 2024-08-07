using Microsoft.EntityFrameworkCore;
using Prod.Entity;

namespace Prod.DB

{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProductInfo> ProductInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
