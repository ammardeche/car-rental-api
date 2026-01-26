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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // safer than UserManager here
            var booking = await _bookingService.CreateBookingAsync(userId, dto.CarId, dto.StartDate, dto.EndDate);
            return Ok(booking);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

    }
}