using JetStoreAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JetStoreAPI.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
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

            builder.Entity<AppUser>()
               .HasMany(ur => ur.UserRoles)
               .WithOne(u => u.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();

            builder.Entity<AppRole>()

                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

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
