
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wasla.DAL.Identity;
using Wasla.Models;
//using Wasla.Models;

namespace Wasla
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<CourierCompany> CourierCompanies { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<TrackingHistory> TrackingHistories { get; set; }
        public DbSet<RateCard> RateCards { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<DriverVehicle> DriverVehicles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
