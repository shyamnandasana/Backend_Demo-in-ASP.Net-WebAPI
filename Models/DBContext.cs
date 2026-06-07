using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Ticket_Comment> Ticket_Comments { get; set; }

    public virtual DbSet<Ticket_Status_Log> Ticket_Status_Logs { get; set; }

    public virtual DbSet<User> Users { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC077169DBC4");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC07005A3380");

            entity.Property(e => e.Created_At).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Priority).HasDefaultValue("MEDIUM");
            entity.Property(e => e.Status).HasDefaultValue("OPEN");

            entity.HasOne(d => d.Assigned_ToNavigation).WithMany(p => p.TicketAssigned_ToNavigations)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Tickets_Assigned");

            entity.HasOne(d => d.Created_ByNavigation).WithMany(p => p.TicketCreated_ByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Creator");
        });

        modelBuilder.Entity<Ticket_Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket_C__3214EC07E96EC326");

            entity.Property(e => e.Created_At).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Ticket_Comments).HasConstraintName("FK_Comments_Ticket");

            entity.HasOne(d => d.User).WithMany(p => p.Ticket_Comments).HasConstraintName("FK_Comments_User");
        });

        modelBuilder.Entity<Ticket_Status_Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket_S__3214EC0778B580C3");

            entity.Property(e => e.Changed_At).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Changed_ByNavigation).WithMany(p => p.Ticket_Status_Logs).HasConstraintName("FK_Logs_User");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Ticket_Status_Logs).HasConstraintName("FK_Logs_Ticket");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07973C8424");

            entity.Property(e => e.Created_At).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
