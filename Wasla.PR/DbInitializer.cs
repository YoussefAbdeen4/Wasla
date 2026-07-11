using Microsoft.AspNetCore.Identity;
using Wasla.DAL.Identity; 
using Wasla.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Wasla.PR
{
    public static class DbInitializer
    {
        public static async Task SeedSuperAdminsAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var adminsToSeed = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "abdeenyoussef9@gmail.com", Email = "abdeenyoussef9@gmail.com", PhoneNumber = "01281854108", EmailConfirmed = true, UserType = UserType.Admin },
                new ApplicationUser { UserName = "mustafadarweish136@gmail.com", Email = "mustafadarweish136@gmail.com", PhoneNumber = "01068459779", EmailConfirmed = true, UserType = UserType.Admin },
                new ApplicationUser { UserName = "Os2739025@gmail.com", Email = "Os2739025@gmail.com", PhoneNumber = "01010742966", EmailConfirmed = true, UserType = UserType.Admin },
                new ApplicationUser { UserName = "Doaaabdelnasser2004@gmail.com", Email = "Doaaabdelnasser2004@gmail.com", PhoneNumber = "+20 10 80594937", EmailConfirmed = true, UserType = UserType.Admin },
                new ApplicationUser { UserName = "msheky227@gmail.com", Email = "msheky227@gmail.com", PhoneNumber = "01025287074", EmailConfirmed = true, UserType = UserType.Admin }
            };

            string defaultPassword = "Password@1234";

            foreach (var admin in adminsToSeed)
            {
                var existingUser = await userManager.FindByEmailAsync(admin.Email);
                if (existingUser == null)
                {
                    var createResult = await userManager.CreateAsync(admin, defaultPassword);
                    if (createResult.Succeeded)
                    {
                        // بنربطه بالـ Role برضه عشان الـ [Authorize] تشتغل صح
                        await userManager.AddToRoleAsync(admin, adminRole);
                    }
                }
            }
        }
    }
}