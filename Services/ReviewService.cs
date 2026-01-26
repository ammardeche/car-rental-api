using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.data;
using CarRental.Api.DTOs;
using CarRental.Api.Interfaces;
using CarRental.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ApplicationDbContext _context;

        public ReviewService(IReviewRepository reviewRepository, ApplicationDbContext context)
        {
            _context = context;
            _reviewRepository = reviewRepository;
        }
        public async Task<Review> CreateAsync(string userId, CreateReviewDto dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new Exception("Rating must be between 1 and 5.");

            var carExists = await _context.Cars.AnyAsync(c => c.Id == dto.CarId);
            if (!carExists)
                throw new Exception("Car not found.");

            var alreadyReviewed = await _reviewRepository.ExistsAsync(userId, dto.CarId);
            if (alreadyReviewed)
                throw new Exception("You already reviewed this car.");

            var review = new Review
            {
                UserId = userId,
                CarId = dto.CarId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();

            return review;
        }

        public async Task<List<Review>> GetByCarAsync(int carId)
        {
            return await _reviewRepository.GetByCarIdAsync(carId);
        }
    }
}