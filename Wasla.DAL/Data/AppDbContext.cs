
using Microsoft.EntityFrameworkCore;
using Wasla.Models;

namespace Wasla
{
    public class AppDbContext : DbContext
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
              modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        }
    }
}
