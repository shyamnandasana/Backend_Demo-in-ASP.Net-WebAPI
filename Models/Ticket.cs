using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Models;

public partial class Ticket
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Description { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Priority { get; set; }

    public int Created_By { get; set; }

    public int? Assigned_To { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Created_At { get; set; }

    [ForeignKey("Assigned_To")]
    [InverseProperty("TicketAssigned_ToNavigations")]
    public virtual User? Assigned_ToNavigation { get; set; }

    [ForeignKey("Created_By")]
    [InverseProperty("TicketCreated_ByNavigations")]
    public virtual User Created_ByNavigation { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<Ticket_Comment> Ticket_Comments { get; set; } = new List<Ticket_Comment>();

    [InverseProperty("Ticket")]
    public virtual ICollection<Ticket_Status_Log> Ticket_Status_Logs { get; set; } = new List<Ticket_Status_Log>();
}
