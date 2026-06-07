using System.ComponentModel.DataAnnotations;

namespace BackendAPI.DTOs
{
    public class TicketStatusDTO
    {
        [Required]
        public string NewStatus { get; set; }
    }
}
