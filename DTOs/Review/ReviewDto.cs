using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Api.DTOs.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}