using ticket_selling_backend.DTOs;
using ticket_selling_backend.Entities;

namespace ticket_selling_backend.Mappers;

public static class EventMappers
{
    public static EventDto ToDto(this Event eventModel)
    {
        return new EventDto
        {
            Id = eventModel.Id,
            Title = eventModel.Title,
            Description = eventModel.Description,
            Date = eventModel.Date,
            Location = eventModel.Location,
            Price = eventModel.Price,
            TotalTickets = eventModel.TotalTickets,
            AvailableTickets = eventModel.AvailableTickets
        };
    }

    public static Event ToEntity(this EventCreateDto eventDto)
    {
        return new Event
        {
            Title = eventDto.Title,
            Description = eventDto.Description,
            Date = eventDto.Date,
            Location = eventDto.Location,
            Price = eventDto.Price,
            TotalTickets = eventDto.TotalTickets,
            AvailableTickets = eventDto.TotalTickets // TODOAl inicio, disponibles = totales
        };
    }
}
