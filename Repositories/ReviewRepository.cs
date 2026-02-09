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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task<bool> ExistsAsync(string userId, int carId)
        {
            return await _context.Reviews.AnyAsync(r => r.UserId == userId && r.CarId == carId);
        }

        public async Task<List<Review>> GetByCarIdAsync(int carId)
        {
            return await _context.Reviews
            .Where(r => r.CarId == carId)
            .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}