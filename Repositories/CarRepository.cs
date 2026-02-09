using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.data;
using CarRental.Api.Interfaces;
using CarRental.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Car> CreateAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            return car;
        }

        public Task DeleteAsync(Car car)
        {
            _context.Cars.Remove(car);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAvailableAsync()
        {
            return await _context.Cars.Where(c => c.isAvailable).ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)

        {
            return await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

        }
        public Task UpdateAsync(Car car)
        {
            _context.Cars.Update(car);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Cars.AnyAsync(c => c.Id == id);
        }


    }
}