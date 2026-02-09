using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.DTOs.Car;

namespace CarRental.Api.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetAllCarsAsync();
        Task<CarDto?> GetCarByIdAsync(int id);
        Task<IEnumerable<CarDto>> GetAvailableCarsAsync();
        Task<CarDto> CreateCarAsync(CreateCarDto dto);
        Task<CarDto> UpdateCarAsync(int id, UpdateCarDto dto);
        Task<bool> DeleteCarAsync(int id);
    }
}