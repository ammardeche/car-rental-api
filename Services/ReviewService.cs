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

            // Check if already reviewed
            var existingReview = await _reviewRepository.ExistsAsync(userId, dto.CarId);
            if (existingReview)
                throw new Exception("You already reviewed this car.");

            // Check if user has a finished booking
            var hasFinishedBooking = await _context.Bookings.AnyAsync(b =>
                b.UserId == userId &&
                b.CarId == dto.CarId &&
                b.EndDate <= DateTime.UtcNow.Date
            );

            if (!hasFinishedBooking)
                throw new Exception("You can review only cars you have already rented.");

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

        public async Task<bool> CanUserReviewAsync(string userId, int carId)
        {
            // Check if already reviewed
            var alreadyReviewed = await _reviewRepository.ExistsAsync(userId, carId);
            if (alreadyReviewed)
                return false;

            // Check if user has a finished booking
            var hasFinishedBooking = await _context.Bookings.AnyAsync(b =>
                b.UserId == userId &&
                b.CarId == carId &&
                b.EndDate <= DateTime.UtcNow.Date
            );

            return hasFinishedBooking;
        }

        public async Task<List<Review>> GetByCarAsync(int carId)
        {
            return await _reviewRepository.GetByCarIdAsync(carId);
        }
    }
}
