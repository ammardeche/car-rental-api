using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRental.Api.DTOs.Booking;
using CarRental.Api.Interfaces;
using CarRental.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingController : ControllerBase
    {

        private readonly IBookingService _bookingService;
        private readonly UserManager<User> _userManager;

        public BookingController(
            IBookingService bookingService,
            UserManager<User> userManager)
        {
            _bookingService = bookingService;
            _userManager = userManager;
        }

        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBooking(CreateBookingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // safer than UserManager here
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated.");
            }

            var booking = await _bookingService.CreateBookingAsync(userId, dto.CarId, dto.StartDate, dto.EndDate);

            // Map to DTO to avoid serialization cycles
            var bookingDto = MapToDto(booking);

            return CreatedAtAction(nameof(GetById), new { id = bookingDto.Id }, bookingDto);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            var dtos = bookings.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(MapToDto(booking));
        }
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated.");
            }

            var bookings = await _bookingService.GetByUserIdAsync(userId);
            var dtos = bookings.Select(MapToDto);
            return Ok(dtos);
        }
        private BookingDto MapToDto(Booking booking)
        {
            var carDto = booking.Car == null ? null : new CarRental.Api.DTOs.Car.CarDto
            {
                Id = booking.Car.Id,
                Brand = booking.Car.Brand,
                Model = booking.Car.Model,
                Year = booking.Car.Year,
                PricePerDay = booking.Car.PricePerDay,
                IsAvailable = booking.Car.isAvailable,
                ImageUrl = booking.Car.ImageUrl
            };

            return new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                UserEmail = booking.User?.Email,
                Car = carDto,
                CardId = booking.CardId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                TotalPrice = booking.TotalPrice
            };
        }
    }
}