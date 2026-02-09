using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.DTOs.Car;
using CarRental.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        // GET: api/Car
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        // GET: api/Car/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var cars = await _carService.GetAvailableCarsAsync();
            return Ok(cars);
        }

        // GET: api/Car/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        // POST: api/Car
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCarDto dto)
        {
            var car = await _carService.CreateCarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
        }

        // PUT: api/Car/5
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCarDto dto)
        {
            try
            {
                var updatedCar = await _carService.UpdateCarAsync(id, dto);
                return Ok(updatedCar);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Car/5
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _carService.DeleteCarAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsPath = Path.Combine("wwwroot/images/cars");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var imageUrl = $"/images/cars/{fileName}";

            // ✅ Return as JSON object with property ImageUrl
            return Ok(new { imageUrl });
        }

    }
}
