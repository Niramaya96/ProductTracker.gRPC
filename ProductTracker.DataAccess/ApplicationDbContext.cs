﻿using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> ProductInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-UNNMCAA;Database=StationeryShop;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Бумага/упак", Price = 199, Count = 3 },
                new Product() { Id = 2, Name = "Ножницы/шт", Price = 150, Count = 10 },
                new Product() { Id = 3, Name = "Карандаши/упак", Price = 100, Count = 7 });
        }
    }
