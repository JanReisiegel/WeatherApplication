using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Weather.Models;

namespace Weather.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MyForecastItem> MyForecasts { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SavedLocation> SavedLocations { get; set; }
        public DbSet<MyWeatherForecast> MyWeatherForecasts { get; set;}
        public DbSet<MyWeatherInfo> MyWeatherInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasIndex(x => new { x.Latitude, x.Longitude }).IsUnique();
            });
            modelBuilder.Entity<SavedLocation>(entity =>
            {
                entity.HasOne(x => x.User).WithMany(x => x.SavedLocations).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Location).WithMany(x => x.SavedLocations).HasForeignKey(x => x.LocationId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<MyWeatherForecast>(entity =>
            {
                entity.HasMany(x => x.ForecastItems).WithOne(x => x.Forecast).HasForeignKey(x => x.ForecastId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Location).WithMany(x => x.MyWeatherForecasts).HasForeignKey(x => x.LocationId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<MyWeatherInfo>(entity =>
            {
                entity.HasOne(x => x.Location).WithMany(x => x.MyWeatherInfos).HasForeignKey(x => x.LocationId).OnDelete(DeleteBehavior.Cascade);
            });

            var hasher = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@weather-forecats.com",
                NormalizedEmail = "ADMIN@WEATHER-FOReCAST.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PasswordHash = hasher.HashPassword(null, "Admin123"),
                SecurityStamp = string.Empty,
                PaidAccount = true
            });
        }

    }
}
