using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ticket_selling_backend.Data;
using ticket_selling_backend.Entities;

namespace ticket_selling_backend.Extensions;

public static class SeederExtension
{
    public static async Task SeedRolesAndDataAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await dbContext.Database.MigrateAsync();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

        string[] roles = { "Admin", "User" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new RoleEntity
                {
                    Name = roleName
                };
                await roleManager.CreateAsync(role);
            }
        }

        var adminEmail = "admin@ticketsystem.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new UserEntity
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "General",
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        if (!await dbContext.Categories.AnyAsync())
        {
            var category = new Category { Name = "Conciertos" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();

            if (!await dbContext.Events.AnyAsync())
            {
                var events = new List<Event>
                {
                    new Event { Title = "Concierto Rock", Description = "Banda en vivo", Date = DateTime.Now.AddDays(10), Location = "Estadio Nacional", Price = 50.00m, TotalTickets = 100, AvailableTickets = 100, CategoryId = category.Id },
                    new Event { Title = "Festival Indie", Description = "Varias bandas", Date = DateTime.Now.AddDays(20), Location = "Parque Central", Price = 30.00m, TotalTickets = 200, AvailableTickets = 200, CategoryId = category.Id },
                    new Event { Title = "Sinfonía Clásica", Description = "Orquesta en vivo", Date = DateTime.Now.AddDays(30), Location = "Teatro Municipal", Price = 80.00m, TotalTickets = 50, AvailableTickets = 50, CategoryId = category.Id },
                    new Event { Title = "Show de Comedia", Description = "Stand up comedy", Date = DateTime.Now.AddDays(40), Location = "Auditorio Sur", Price = 25.00m, TotalTickets = 150, AvailableTickets = 150, CategoryId = category.Id },
                    new Event { Title = "Noche de Jazz", Description = "Música suave", Date = DateTime.Now.AddDays(50), Location = "Bar Centro", Price = 20.00m, TotalTickets = 80, AvailableTickets = 80, CategoryId = category.Id }
                };
                
                dbContext.Events.AddRange(events);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
