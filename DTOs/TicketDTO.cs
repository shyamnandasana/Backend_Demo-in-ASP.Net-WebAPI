using System.ComponentModel.DataAnnotations;

namespace BackendAPI.DTOs

{ 
        public class TicketDTO
        {
            [Required]
            [MinLength(5)]
            public string Title { get; set; }

            [Required]
            [MinLength(10)]
            public string Description { get; set; }

            [Required]
            public string Priority { get; set; } = "MEDIUM";
        }
}

