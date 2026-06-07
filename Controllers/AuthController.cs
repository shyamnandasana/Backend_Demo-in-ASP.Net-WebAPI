using BackendAPI.Models;
using BackendAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IConfiguration _config;

        public AuthController(DBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: /auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            // Verify password using BCrypt
            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!validPassword)
                return Unauthorized("Invalid credentials");

            // Generate JWT
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token, Role = user.Role!.Name });
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var claims = new[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            //    new Claim("nameid", user.Id.ToString()),
            //    new Claim("role", user.Role!.Name),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};
            var claims = new[]
            {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // standard for user ID
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role!.Name),             // standard for roles
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}