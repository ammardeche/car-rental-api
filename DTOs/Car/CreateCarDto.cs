using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Api.DTOs.Car
{
    public class CreateCarDto
    {
        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        public decimal PricePerDay { get; set; }

        public string? ImageUrl { get; set; }
    }
}