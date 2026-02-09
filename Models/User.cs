using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Api.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}