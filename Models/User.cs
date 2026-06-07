using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Models;

[Index("Email", Name = "UQ__Users__A9D10534CC9E4AF8", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    public int Role_Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Created_At { get; set; }

    [ForeignKey("Role_Id")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("Assigned_ToNavigation")]
    public virtual ICollection<Ticket> TicketAssigned_ToNavigations { get; set; } = new List<Ticket>();

    [InverseProperty("Created_ByNavigation")]
    public virtual ICollection<Ticket> TicketCreated_ByNavigations { get; set; } = new List<Ticket>();

    [InverseProperty("User")]
    public virtual ICollection<Ticket_Comment> Ticket_Comments { get; set; } = new List<Ticket_Comment>();

    [InverseProperty("Changed_ByNavigation")]
    public virtual ICollection<Ticket_Status_Log> Ticket_Status_Logs { get; set; } = new List<Ticket_Status_Log>();
}
