using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Models;

public partial class Ticket_Status_Log
{
    [Key]
    public int Id { get; set; }

    public int Ticket_Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Old_Status { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string New_Status { get; set; } = null!;

    public int Changed_By { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Changed_At { get; set; }

    [ForeignKey("Changed_By")]
    [InverseProperty("Ticket_Status_Logs")]
    public virtual User Changed_ByNavigation { get; set; } = null!;

    [ForeignKey("Ticket_Id")]
    [InverseProperty("Ticket_Status_Logs")]
    public virtual Ticket Ticket { get; set; } = null!;
}
