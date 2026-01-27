using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.Models;

namespace CarRental.Api.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(
                string userId,
                int carId,
                DateTime startDate,
                DateTime endDate


            );

        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
    }
}