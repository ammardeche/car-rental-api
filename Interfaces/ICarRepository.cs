using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.Models;

namespace CarRental.Api.Interfaces
{
    public interface ICarRepository
    {

        Task<IEnumerable<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(int id);
        Task<IEnumerable<Car>> GetAvailableAsync();

        Task<Car> CreateAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(Car car);

        Task<bool> ExistsAsync(int id);
        Task SaveAsync();
    }
}