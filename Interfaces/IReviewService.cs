using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.DTOs;
using CarRental.Api.Models;

namespace CarRental.Api.Interfaces
{
    public interface IReviewService
    {
        Task<Review> CreateAsync(string userId, CreateReviewDto dto);
        Task<List<Review>> GetByCarAsync(int carId);
    }
}