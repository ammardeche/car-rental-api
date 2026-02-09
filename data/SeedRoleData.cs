using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.data
{
    public static class SeedRoleData
    {
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "11111111-1111-1111-1111-111111111111",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "22222222-2222-2222-2222-222222222222",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                }
            );
        }
    }
}