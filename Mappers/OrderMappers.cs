using ticket_selling_backend.DTOs;
using ticket_selling_backend.Entities;

namespace ticket_selling_backend.Mappers;

public static class OrderMappers
{
    public static object ToDto(this Order order)
    {
        return new
        {
            Id = order.Id,
            TotalAmount = order.TotalAmount,
            PaymentStatus = order.PaymentStatus,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new 
            {
                EventId = i.EventId,
                EventTitle = i.Event?.Title,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            })
        };
    }
}
