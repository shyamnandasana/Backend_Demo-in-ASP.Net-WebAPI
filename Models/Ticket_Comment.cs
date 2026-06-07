using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Models;

public partial class Ticket_Comment
{
    [Key]
    public int Id { get; set; }

    public int Ticket_Id { get; set; }

    public int User_Id { get; set; }

    [Column(TypeName = "text")]
    public string Comment { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Created_At { get; set; }

    [ForeignKey("Ticket_Id")]
    [InverseProperty("Ticket_Comments")]
    public virtual Ticket Ticket { get; set; } = null!;

    [ForeignKey("User_Id")]
    [InverseProperty("Ticket_Comments")]
    public virtual User User { get; set; } = null!;
}
