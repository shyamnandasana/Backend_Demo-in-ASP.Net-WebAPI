using BackendAPI.Models;
using BackendAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsAPIController : ControllerBase
    {
      
            private readonly DBContext _context;

            public CommentsAPIController(DBContext context)
            {
                _context = context;
            }

           
            [HttpPatch("{id}")]
            [Authorize(Roles = "MANAGER,USER,SUPPORT")]
            public async Task<IActionResult> UpdateComment(int id, CommentDTO dto)
            {
                var comment = await _context.Ticket_Comments.FindAsync(id);
                if (comment == null) return NotFound("Comment not found");

                var currentUserId = int.Parse(User.FindFirst("nameid")!.Value);
                var currentUserRole = User.FindFirst("role")!.Value;

                // Only MANAGER or comment author can update
                if (currentUserRole != "MANAGER" && comment.User_Id != currentUserId)
                    return Forbid("You are not allowed to update this comment");

                // Update comment
                comment.Comment = dto.Comment;
                comment.Created_At = DateTime.Now;

                await _context.SaveChangesAsync();
                return Ok(comment);
            }

            // DELETE /comments/{id}
            [HttpDelete("{id}")]
            [Authorize(Roles = "MANAGER,USER,SUPPORT")]
            public async Task<IActionResult> DeleteComment(int id)
            {
                var comment = await _context.Ticket_Comments.FindAsync(id);
                if (comment == null) return NotFound("Comment not found");

                var currentUserId = int.Parse(User.FindFirst("nameid")!.Value);
                var currentUserRole = User.FindFirst("role")!.Value;

                // Only MANAGER or comment author can delete
                if (currentUserRole != "MANAGER" && comment.User_Id != currentUserId)
                    return Forbid("You are not allowed to delete this comment");

                _context.Ticket_Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

 
