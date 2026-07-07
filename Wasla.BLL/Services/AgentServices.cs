using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Wasla.DAL.Identity;
using Wasla.Enums;
using Wasla.Models;

namespace Wasla.BLL.Services
{
    public class AgentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AgentService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Driver>> GetAgentsAsync(int companyId)
        {
            return await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.DriverOrders)
                .Where(d => d.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task CreateAsync(Driver driver, Vehicle vehicle, string password, string companyUserId)
        {
            var company = await _context.CourierCompanies
                .FirstOrDefaultAsync(x => x.UserId == companyUserId);

            if (company == null)
                throw new Exception("Company not found");

            if (driver.User == null)
                throw new Exception("User data is missing.");

            driver.User.UserType = UserType.Driver;
            driver.User.UserName = driver.User.Email;

            var result = await _userManager.CreateAsync(driver.User, password);

            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine,
                    result.Errors.Select(x => x.Description)));

            driver.UserId = driver.User.Id;
            driver.CompanyId = company.Id;
            driver.TotalCashSubmitted = 0;

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            vehicle.CompanyId = company.Id;
            vehicle.IsActive = vehicle.IsActive;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            var driverVehicle = new DriverVehicle
            {
                DriverId = driver.Id,
                VehicleId = vehicle.Id,
                AssignedAt = DateTime.Now
            };

            _context.DriverVehicles.Add(driverVehicle);

            await _context.SaveChangesAsync();
        }

        public async Task<Driver?> GetByIdAsync(int driverId, int companyId)
        {
            return await _context.Drivers
                 .AsNoTracking()
                 .Include(d => d.User)
                 .Include(d => d.DriverOrders)
                    .ThenInclude(x => x.Order)
                 .Include(d => d.DriverVehicles)
                    .ThenInclude(dv => dv.Vehicle)   
                 .FirstOrDefaultAsync(d => d.Id == driverId && d.CompanyId == companyId);
        }

        public async Task UpdateAsync(Driver model, string companyUserId)
        {
            var company = await _context.CourierCompanies
                .FirstOrDefaultAsync(c => c.UserId == companyUserId);

            if (company == null)
                throw new Exception("Company not found");

            var driver = await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.Phones)
                .Include(d => d.DriverVehicles)
                    .ThenInclude(dv => dv.Vehicle)
                .FirstOrDefaultAsync(d => d.Id == model.Id && d.CompanyId == company.Id);

            if (driver == null)
                throw new Exception("Driver not found");

            // Driver
            driver.Name = model.Name;

            // Identity User
            driver.User.Email = model.User.Email;
            driver.User.UserName = model.User.UserName;
            driver.User.NormalizedEmail = model.User.Email.ToUpper();
            driver.User.NormalizedUserName = model.User.UserName.ToUpper();
            driver.User.PhoneNumber = model.User.PhoneNumber;

            //// Phone
            //if (driver.Phones.Any() && model.Phones.Any())
            //{
            //    driver.Phones.First().PhoneNumber = model.Phones.First().PhoneNumber;
            //}

            // Vehicle
            var currentVehicle = driver.DriverVehicles.FirstOrDefault()?.Vehicle;
            var newVehicle = model.DriverVehicles.FirstOrDefault()?.Vehicle;

            if (currentVehicle != null && newVehicle != null)
            {
                currentVehicle.Type = newVehicle.Type;
                currentVehicle.LicensePlate = newVehicle.LicensePlate;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int driverId, int companyId)
        {
            var driver = await _context.Drivers
                .Include(d => d.DriverOrders)
                    .ThenInclude(o => o.Order)
                .FirstOrDefaultAsync(d => d.Id == driverId && d.CompanyId == companyId);

            if (driver == null)
                return false;

            // 🚫 check active orders
            var hasActiveOrders = driver.DriverOrders.Any(o =>
                o.Order.status != OrderStatus.Delivered &&
                o.Order.status != OrderStatus.Cancelled &&
                o.Order.status != OrderStatus.Returned
            );

            if (hasActiveOrders)
                throw new Exception("لا يمكن حذف المندوب لوجود طلبات نشطة");

            // 🧹 fetch DriverVehicles safely
            var driverVehicles = await _context.DriverVehicles
                .Where(x => x.DriverId == driverId)
                .ToListAsync();

            if (driverVehicles.Any())
                _context.DriverVehicles.RemoveRange(driverVehicles);

            // 🗑 delete driver
            _context.Drivers.Remove(driver);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}