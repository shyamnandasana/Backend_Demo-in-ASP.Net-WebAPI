using BackendAPI.DTOs;
using BackendAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
        private readonly DBContext _context;

        public UsersAPIController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                 .Select(u => new UserDTO
                 {
                     Id = u.Id,
                     Name = u.Name,
                     Email = u.Email,
                     Password = u.Password,
                     RoleId = u.Role_Id

                 })
                 .ToListAsync();

            return Ok(users);
        }

        // POST /users
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password), // Hash password
                Role_Id = dto.RoleId,
                Created_At = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

    }
}
