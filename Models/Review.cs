using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Api.Models
{
    public class Review
    {
        public int Id { get; set; }

        // Relations
        public string UserId { get; set; } = null!;
        public User? User { get; set; }

        public int CarId { get; set; }
        public Car? Car { get; set; }

        public int Rating { get; set; } // 1–5
        public string Comment { get; set; } = string.Empty;
    }
}