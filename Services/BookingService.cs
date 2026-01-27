using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.data;
using CarRental.Api.Interfaces;
using CarRental.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ApplicationDbContext _context;


        public BookingService(IBookingRepository bookingRepository, ApplicationDbContext context)
        {
            _bookingRepository = bookingRepository;
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(string userId, int carId, DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ArgumentException("End date must be after start date.");

            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                throw new Exception("Car not found.");

            int rentalDays = (endDate - startDate).Days;
            decimal totalPrice = rentalDays * car.PricePerDay;

            var booking = new Booking
            {
                UserId = userId,
                CarId = carId,

                StartDate = startDate,
                EndDate = endDate,
                TotalPrice = totalPrice
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            // Load navigation properties so returned entity has related data for mapping
            await _context.Entry(booking).Reference(b => b.Car).LoadAsync();
            await _context.Entry(booking).Reference(b => b.User).LoadAsync();

            return booking;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }
    }
}