using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Domain;

namespace ShopTARgv24.Data
{
    public class ShopTARgv24Context : DbContext
    {
        public ShopTARgv24Context(DbContextOptions<ShopTARgv24Context> options)
        : base(options) { }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
        public DbSet<Kindergarten> Kindergartens { get; set; }
        public DbSet<FileToDatabase> KindergartenFileToDatabase { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<FileToDatabase>()
             .HasOne(x => x.Kindergarten)
             .WithMany(x => x.Files)
             .HasForeignKey(x => x.KindergartenId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}