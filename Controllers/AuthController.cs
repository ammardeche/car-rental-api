using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.DTOs.Auth;
using CarRental.Api.Interfaces;
using CarRental.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email))
                return Unauthorized("Invalid email or password.");

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password ?? string.Empty);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid Credential");
            }
            // Generate token
            var token = await _tokenService.CreateTokenAsync(user);

            return Ok(new { Token = token });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Email) || await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("Email is already registered.");
            }

            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FullName = registerDto.FullName ?? string.Empty
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password ?? string.Empty);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assign "User" role by default
            await _userManager.AddToRoleAsync(user, "Customer");

            var token = await _tokenService.CreateTokenAsync(user);

            return Ok(new { Token = token });
        }
    }
}