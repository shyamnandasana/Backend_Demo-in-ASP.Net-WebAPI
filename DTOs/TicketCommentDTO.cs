using System.ComponentModel.DataAnnotations;

namespace BackendAPI.DTOs
{
    public class TicketCommentDTO
    {
        [Required]
        [MinLength(1)]
        public string Comment { get; set; }
    }
}
