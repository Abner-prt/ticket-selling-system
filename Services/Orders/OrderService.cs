using Microsoft.EntityFrameworkCore;
using ticket_selling_backend.Data;
using ticket_selling_backend.DTOs;
using ticket_selling_backend.DTOs.Orders;
using ticket_selling_backend.Entities;
using ticket_selling_backend.Mappers;

namespace ticket_selling_backend.Services.Orders;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseDto<object>> CheckoutAsync(string userId, CheckoutRequestDto dto)
    {
        if (dto.Items == null || !dto.Items.Any())
        {
            return new ResponseDto<object> { StatusCode = 400, Status = false, Message = "No hay items en el carrito." };
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var order = new Order
            {
                UserId = userId,
                TotalAmount = 0,
                PaymentStatus = "Completed", // Hardcoded for this phase
                Items = new List<OrderItem>()
            };

            foreach (var item in dto.Items)
            {
                var ev = await _context.Events.FindAsync(item.EventId);
                if (ev == null)
                {
                    return new ResponseDto<object> { StatusCode = 404, Status = false, Message = $"El evento {item.EventId} no existe." };
                }

                if (ev.AvailableTickets < item.Quantity)
                {
                    return new ResponseDto<object> { StatusCode = 400, Status = false, Message = $"No hay suficientes boletos para {ev.Title}. Disponibles: {ev.AvailableTickets}" };
                }

                ev.AvailableTickets -= item.Quantity;

                var orderItem = new OrderItem
                {
                    EventId = ev.Id,
                    Quantity = item.Quantity,
                    UnitPrice = ev.Price
                };

                order.Items.Add(orderItem);
                order.TotalAmount += (item.Quantity * ev.Price);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();

            return new ResponseDto<object> { StatusCode = 201, Status = true, Message = "Orden creada exitosamente", Data = order.ToDto() };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ResponseDto<object> { StatusCode = 500, Status = false, Message = "Error al procesar la compra: " + ex.Message };
        }
    }

    public async Task<ResponseDto<IEnumerable<object>>> GetMyOrdersAsync(string userId)
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Event)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        var dtos = orders.Select(o => o.ToDto());
        return new ResponseDto<IEnumerable<object>> { StatusCode = 200, Status = true, Data = dtos };
    }
}
