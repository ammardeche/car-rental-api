using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Api.data
{
    public static class SeedUsers
    {
        public async static Task SeedDefaultAdminUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            // 2. Check if admin already exists
            string adminEmail = "admin@carrental.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Create default admin
                var user = new User
                {
                    FullName = "Admin",
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin123!"); // default password

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }


        }
    }
}