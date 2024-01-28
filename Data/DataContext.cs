using JetStoreAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JetStoreAPI.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { 
        }
        public DbSet<Item> Items  => Set<Item>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<MeasureUnit> MeasureUnits => Set<MeasureUnit>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Feature>()
                //.HasMany(item => item.F)
                .HasOne(feature => feature.Item)
                .WithMany(x => x.Features)
               // .HasForeignKey(feature => feature.Item)
                .OnDelete(DeleteBehavior.Cascade)
                ;


        }
    }
}
