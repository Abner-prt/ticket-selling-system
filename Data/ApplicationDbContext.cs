using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ticket_selling_backend.Entities;

namespace ticket_selling_backend.Data;

// Hereda de IdentityDbContext para incluir las tablas de Identity
public class ApplicationDbContext : IdentityDbContext<UserEntity, RoleEntity, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public DbSet<Event> Events { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Category> Categories { get; set; }
}
