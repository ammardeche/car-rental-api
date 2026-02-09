using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.Models;

namespace CarRental.Api.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
    }
}