using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.DTOs.Car;
using CarRental.Api.Interfaces;
using CarRental.Api.Models;

namespace CarRental.Api.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        public async Task<CarDto> CreateCarAsync(CreateCarDto dto)
        {
            if (dto.PricePerDay < 0)
                throw new ArgumentException("Price must be >= 0");

            var car = new Car
            {
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                PricePerDay = dto.PricePerDay,
                ImageUrl = dto.ImageUrl,
                isAvailable = true
            };

            await _carRepository.CreateAsync(car);
            await _carRepository.SaveAsync();

            return new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                PricePerDay = car.PricePerDay,
                IsAvailable = car.isAvailable,
                ImageUrl = car.ImageUrl
            };
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
                return false;

            await _carRepository.DeleteAsync(car);
            await _carRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetAllAsync();

            return cars.Select(c => new CarDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                PricePerDay = c.PricePerDay,
                IsAvailable = c.isAvailable,
                ImageUrl = c.ImageUrl
            });
        }

        public async Task<IEnumerable<CarDto>> GetAvailableCarsAsync()
        {
            var cars = await _carRepository.GetAvailableAsync();
            return cars.Select(c => new CarDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                PricePerDay = c.PricePerDay,
                IsAvailable = c.isAvailable,
                ImageUrl = c.ImageUrl
            });
        }

        public async Task<CarDto?> GetCarByIdAsync(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null) return null;

            return new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                PricePerDay = car.PricePerDay,
                IsAvailable = car.isAvailable,
                ImageUrl = car.ImageUrl
            };
        }

        public async Task<CarDto> UpdateCarAsync(int id, UpdateCarDto dto)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
                throw new KeyNotFoundException("Car not found");

            // Business logic: prevent negative price
            if (dto.PricePerDay < 0)
                throw new ArgumentException("Price must be >= 0");

            car.Brand = dto.Brand;
            car.Model = dto.Model;
            car.Year = dto.Year;
            car.PricePerDay = dto.PricePerDay;
            car.isAvailable = dto.IsAvailable;
            car.ImageUrl = dto.ImageUrl;

            await _carRepository.UpdateAsync(car);
            await _carRepository.SaveAsync();

            return new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                PricePerDay = car.PricePerDay,
                IsAvailable = car.isAvailable,
                ImageUrl = car.ImageUrl
            };
        }
    }
}