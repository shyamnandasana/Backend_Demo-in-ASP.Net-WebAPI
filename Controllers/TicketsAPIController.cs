using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendAPI.Models;
using BackendAPI.DTOs;
using System.Security.Claims;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("tickets")]
    [Authorize] 
    public class TicketsAPIController : ControllerBase
    {
        private readonly DBContext _context;
        public TicketsAPIController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "USER,MANAGER")]
        public async Task<IActionResult> CreateTicket(TicketDTO dto)
        {
            var userIdClaim = User.FindFirst("nameid");
            if (userIdClaim == null)
                return Unauthorized("User not found");

            int userId = int.Parse(userIdClaim.Value);

            Ticket ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = "OPEN",
                Created_By = userId,
                Created_At = DateTime.Now
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Created($"/tickets/{ticket.Id}", ticket);
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || roleClaim == null)
                return Unauthorized("Invalid token claims");

            int userId = int.Parse(userIdClaim.Value);
            string role = roleClaim.Value;

            var ticketsQuery = _context.Tickets
                .Include(t => t.Assigned_ToNavigation)
                .Include(t => t.Created_ByNavigation)
                .AsQueryable();

            if (role == "USER")
                ticketsQuery = ticketsQuery.Where(t => t.Created_By == userId);
            else if (role == "SUPPORT")
                ticketsQuery = ticketsQuery.Where(t => t.Assigned_To == userId);

            var tickets = await ticketsQuery.ToListAsync();
            return Ok(tickets);
        }

        [HttpPatch("{id}/assign")]
        [Authorize(Roles = "MANAGER,SUPPORT")]
        public async Task<IActionResult> AssignTicket(int id, TicketAssignDTO dto)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            var user = await _context.Users.FindAsync(dto.AssignedTo);
            if (user == null) return NotFound("User not found");
            if (user.Role_Id == 3) return BadRequest("Cannot assign to USER role");

            ticket.Assigned_To = dto.AssignedTo;
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "MANAGER,SUPPORT")]
        public async Task<IActionResult> UpdateStatus(int id, TicketStatusDTO dto)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            if (!IsValidTransition(ticket.Status, dto.NewStatus))
                return BadRequest("Invalid status transition");

            var log = new Ticket_Status_Log
            {
                Ticket_Id = id,
                Old_Status = ticket.Status,
                New_Status = dto.NewStatus,
                Changed_By = int.Parse(User.FindFirst("nameid")!.Value),
                Changed_At = DateTime.Now
            };

            ticket.Status = dto.NewStatus;
            _context.Ticket_Status_Logs.Add(log);
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool IsValidTransition(string oldStatus, string newStatus)
        {
            return (oldStatus, newStatus) switch
            {
                ("OPEN", "IN_PROGRESS") => true,
                ("IN_PROGRESS", "RESOLVED") => true,
                ("RESOLVED", "CLOSED") => true,
                _ => false
            };
        }
    }
}