using System;
using CarRental.Api.DTOs.Car;

namespace CarRental.Api.DTOs.Booking
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public CarDto? Car { get; set; }
        public int? CardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}