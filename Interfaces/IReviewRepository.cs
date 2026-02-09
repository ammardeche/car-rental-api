using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.Models;

namespace CarRental.Api.Interfaces
{
    public interface IReviewRepository
    {
        Task AddAsync(Review review);
        Task<bool> ExistsAsync(string userId, int carId);
        Task<List<Review>> GetByCarIdAsync(int carId);
        Task SaveChangesAsync();
    }
}