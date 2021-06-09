﻿using Microsoft.EntityFrameworkCore;

namespace NorthwindApp.Models
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.CategoryId);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(x => x.SupplierId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.ProductId);

                entity
                    .HasOne(product => product.Supplier)
                    .WithMany(supplier => supplier.Products)
                    .HasForeignKey(product => product.SupplierId)
                    .HasConstraintName("FK_Products_Suppliers");

                entity
                    .HasOne(x => x.Category)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.CategoryId)
                    .HasConstraintName("FK_Products_Categories");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
