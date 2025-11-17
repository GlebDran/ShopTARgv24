using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Domain;
using System.Collections.Generic;


namespace ShopTARgv24.Data
{
    public class ShopTARgv24Context : DbContext
    {
        public ShopTARgv24Context(DbContextOptions<ShopTARgv24Context> options)
        : base(options) { }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
        public DbSet<RealEstate> RealEstate { get; set; }
        public DbSet<FileToDatabase> FileToDatabases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileToDatabase>()
                .HasOne(f => f.RealEstate)
                .WithMany(r => r.Files)
                .HasForeignKey(f => f.RealEstateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}