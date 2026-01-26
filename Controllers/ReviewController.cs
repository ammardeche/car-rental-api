using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRental.Api.DTOs;
using CarRental.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var review = await _reviewService.CreateAsync(userId, dto);
            return Ok(review);
        }

        [HttpGet("car/{carId}")]
        public async Task<IActionResult> GetByCar(int carId)
        {
            var reviews = await _reviewService.GetByCarAsync(carId);
            return Ok(reviews);
        }
    }
}