using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenWeatherMap.Models;
using WeatherApi.Models;

namespace WeatherApi.Data
{
    public class AppDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        //weatherforcast items to database
        //weatherInfo to database
        //savedLocation in DBs
        public DbSet<WeatherForecastItem> WeatherForecasts { get; set; }
        public DbSet<WeatherInfo> WeatherInfos { get; set; }
        public DbSet<SavedLocation> SavedLocations { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<IdentityUser>();

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@weather-forcats.com",
                NormalizedEmail = "ADMIN@WEATHER-FORCAST.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PasswordHash = hasher.HashPassword(null, "Admin123"),
                SecurityStamp = string.Empty,
                PaidAccount = true
            });

            builder.Entity<SavedLocation>(entity =>
            {
                entity.HasOne(x => x.User).WithMany(x => x.SavedLocations).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
