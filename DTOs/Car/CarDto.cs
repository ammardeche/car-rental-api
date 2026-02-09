using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Api.DTOs.Car
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}