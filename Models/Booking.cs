using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Api.Models
{
    public class Booking
    {
        public int Id { get; set; }

        // Relations 

        public string UserId { get; set; } = null!;
        public User? User { get; set; }

        public int CardId { get; set; }
        public Car? Car { get; set; }

        // Booking details
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}