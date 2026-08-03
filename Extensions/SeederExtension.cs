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
            var catConciertos = new Category { Name = "Conciertos 🎤" };
            var catDeportes = new Category { Name = "Deportes ⚽" };
            var catTeatro = new Category { Name = "Teatro y Arte 🎭" };
            var catConvenciones = new Category { Name = "Convenciones 🎪" };
            var catEsports = new Category { Name = "E-Sports 🎮" };
            var catFestivales = new Category { Name = "Festivales y Gastronomía 🍔" };
            var catCine = new Category { Name = "Cine y Autocines 🍿" };
            var catFamiliares = new Category { Name = "Familiares e Infantiles 🎈" };
            var catEspeciales = new Category { Name = "Especiales 🌟" };

            dbContext.Categories.AddRange(catConciertos, catDeportes, catTeatro, catConvenciones, catEsports, catFestivales, catCine, catFamiliares, catEspeciales);
            await dbContext.SaveChangesAsync();

            if (!await dbContext.Events.AnyAsync())
            {
                var events = new List<Event>
                {
                    // Conciertos
                    new Event { Title = "World's Hottest Tour - Bad Bunny", Description = "El artista urbano del momento llega al país con su gira mundial.", Date = DateTime.Now.AddDays(15), Location = "Estadio Olímpico", Price = 3500.00m, TotalTickets = 35000, AvailableTickets = 35000, CategoryId = catConciertos.Id },
                    new Event { Title = "Mañana Será Bonito - Karol G", Description = "Una noche inolvidable de reggaeton y pop latino.", Date = DateTime.Now.AddDays(45), Location = "Estadio Chochi Sosa", Price = 2800.00m, TotalTickets = 25000, AvailableTickets = 25000, CategoryId = catConciertos.Id },
                    new Event { Title = "Coldplay Music of the Spheres", Description = "El tour más ecológico y espectacular del mundo.", Date = DateTime.Now.AddDays(90), Location = "Estadio Nacional", Price = 4500.00m, TotalTickets = 40000, AvailableTickets = 40000, CategoryId = catConciertos.Id },
                    
                    // Deportes
                    new Event { Title = "Olimpia vs Motagua - Clásico Nacional", Description = "El partido más esperado de la temporada regular.", Date = DateTime.Now.AddDays(5), Location = "Estadio Nacional Chelato Uclés", Price = 300.00m, TotalTickets = 20000, AvailableTickets = 20000, CategoryId = catDeportes.Id },
                    new Event { Title = "Final Champions League (En Vivo)", Description = "Proyección en pantalla gigante del partido más importante de Europa con fan zone.", Date = DateTime.Now.AddDays(25), Location = "Plaza Central", Price = 150.00m, TotalTickets = 5000, AvailableTickets = 5000, CategoryId = catDeportes.Id },
                    
                    // Teatro
                    new Event { Title = "El Cascanueces - Ballet Nacional", Description = "Un clásico de la danza interpretado por los mejores artistas del país.", Date = DateTime.Now.AddDays(60), Location = "Teatro Nacional Manuel Bonilla", Price = 800.00m, TotalTickets = 800, AvailableTickets = 800, CategoryId = catTeatro.Id },
                    new Event { Title = "Hamilton (Tour Internacional)", Description = "El exitoso musical de Broadway llega en español.", Date = DateTime.Now.AddDays(120), Location = "Teatro Nacional Manuel Bonilla", Price = 2500.00m, TotalTickets = 800, AvailableTickets = 800, CategoryId = catTeatro.Id },
                    
                    // Convenciones
                    new Event { Title = "Comic-Con Honduras 2026", Description = "El evento de cultura pop, cómics y anime más grande de la región.", Date = DateTime.Now.AddDays(35), Location = "Centro de Convenciones", Price = 450.00m, TotalTickets = 10000, AvailableTickets = 10000, CategoryId = catConvenciones.Id },
                    new Event { Title = "Tech Summit Centroamérica", Description = "Conferencias sobre Inteligencia Artificial, Ciberseguridad y Startups.", Date = DateTime.Now.AddDays(80), Location = "Hotel Clarion", Price = 1200.00m, TotalTickets = 1500, AvailableTickets = 1500, CategoryId = catConvenciones.Id },

                    // Más Deportes
                    new Event { Title = "Gran Premio de Miami - Fórmula 1", Description = "Viaje organizado para ver la carrera en vivo desde Miami.", Date = DateTime.Now.AddDays(150), Location = "Miami International Autodrome", Price = 12500.00m, TotalTickets = 100, AvailableTickets = 100, CategoryId = catDeportes.Id },
                    new Event { Title = "Final NBA 2026", Description = "Proyección exclusiva del partido 7 de las Finales de la NBA.", Date = DateTime.Now.AddDays(40), Location = "Sport Bar Universitario", Price = 200.00m, TotalTickets = 300, AvailableTickets = 300, CategoryId = catDeportes.Id },

                    // Especiales
                    new Event { Title = "Rifa Solidaria de Nintendo Switch", Description = "Participa en la rifa de una Nintendo Switch Oled. Lo recaudado será donado a la fundación de niños con cáncer.", Date = DateTime.UtcNow.AddDays(7), Location = "Transmisión en Vivo (Twitch/YouTube)", Price = 15.00m, TotalTickets = 500, AvailableTickets = 500, CategoryId = catEspeciales.Id },

                    // E-Sports
                    new Event { Title = "League of Legends - Worlds 2026 Finals", Description = "Watch Party oficial con mercancía exclusiva y sorteos.", Date = DateTime.Now.AddDays(110), Location = "Cinepolis VIP", Price = 350.00m, TotalTickets = 250, AvailableTickets = 250, CategoryId = catEsports.Id },
                    new Event { Title = "Valorant Champions Tour - Watch Party", Description = "Apoya a tu equipo favorito en pantalla gigante.", Date = DateTime.Now.AddDays(70), Location = "Arena Gamer HN", Price = 150.00m, TotalTickets = 500, AvailableTickets = 500, CategoryId = catEsports.Id },
                    new Event { Title = "Torneo Nacional de Super Smash Bros Ultimate", Description = "Competencia presencial con los mejores jugadores del país. Premio al primer lugar.", Date = DateTime.Now.AddDays(14), Location = "Centro Comercial Multiplaza", Price = 250.00m, TotalTickets = 1000, AvailableTickets = 1000, CategoryId = catEsports.Id },

                    // Festivales y Gastronomía
                    new Event { Title = "Festival de la Cerveza Artesanal", Description = "Degustación de más de 50 cervezas locales con música en vivo.", Date = DateTime.Now.AddDays(20), Location = "Parque España", Price = 300.00m, TotalTickets = 2000, AvailableTickets = 2000, CategoryId = catFestivales.Id },
                    new Event { Title = "Feria del Food Truck HND", Description = "Los mejores camiones de comida del país reunidos en un solo lugar.", Date = DateTime.Now.AddDays(10), Location = "Bulevar Morazán", Price = 100.00m, TotalTickets = 5000, AvailableTickets = 5000, CategoryId = catFestivales.Id },

                    // Cine y Autocines
                    new Event { Title = "Maratón: El Señor de los Anillos (Extendida)", Description = "Trilogía completa proyectada al aire libre. Incluye palomitas ilimitadas.", Date = DateTime.Now.AddDays(50), Location = "Autocine El Picacho", Price = 450.00m, TotalTickets = 150, AvailableTickets = 150, CategoryId = catCine.Id },
                    new Event { Title = "Estreno de Medianoche - Spiderman 4", Description = "Evento exclusivo para fans con alfombra roja y sorpresas.", Date = DateTime.Now.AddDays(90), Location = "Cinepolis VIP", Price = 200.00m, TotalTickets = 300, AvailableTickets = 300, CategoryId = catCine.Id },

                    // Familiares e Infantiles
                    new Event { Title = "Disney On Ice - Sueños Mágicos", Description = "Un espectáculo sobre hielo inolvidable para toda la familia.", Date = DateTime.Now.AddDays(100), Location = "Nacional de Ingenieros Coliseum", Price = 1200.00m, TotalTickets = 4000, AvailableTickets = 4000, CategoryId = catFamiliares.Id },
                    new Event { Title = "El Gran Circo Internacional", Description = "Acróbatas, malabaristas y payasos en la mejor carpa de la ciudad.", Date = DateTime.Now.AddDays(22), Location = "Frente a Mall Multiplaza", Price = 500.00m, TotalTickets = 1500, AvailableTickets = 1500, CategoryId = catFamiliares.Id }
                };
                
                dbContext.Events.AddRange(events);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
