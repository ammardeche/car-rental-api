using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.data;
using CarRental.Api.Interfaces;
using CarRental.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Repositories
{
    public class BookingRepository : IBookingRepository
    {

        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // create a new booking
        public async Task AddAsync(Booking booking)
        => await _context.Bookings.AddAsync(booking);


        // get all bookings with related car and user data
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
            .Include(c => c.Car)
            .Include(u => u.User).ToListAsync();
        }
        // get booking by id with related car and user data
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
            .Include(c => c.Car)
            .Include(u => u.User)
            .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(string userId)
        {
            return await _context.Bookings
     .Include(c => c.Car)
     .Include(u => u.User)
     .Where(b => b.UserId == userId)
     .OrderByDescending(b => b.StartDate)
     .ToListAsync();
        }

        // save changes to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}