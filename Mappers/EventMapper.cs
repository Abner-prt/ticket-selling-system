using ticket_selling_backend.Dtos.Events;
using ticket_selling_backend.Entities;

namespace ticket_selling_backend.Mappers;

public static class EventMapper
{
    public static Event CreateDtoToEntity(EventCreateDto dto)
    {
        return new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date,
            Location = dto.Location,
            Price = dto.Price,
            TotalTickets = dto.TotalTickets,
            AvailableTickets = dto.TotalTickets,
            CategoryId = dto.CategoryId
        };
    }

    public static Event EditDtoToEntity(Event entity, EventEditDto dto)
    {
        entity.Title = dto.Title;
        entity.Description = dto.Description;
        entity.Date = dto.Date;
        entity.Location = dto.Location;
        entity.Price = dto.Price;
        
        int diff = dto.TotalTickets - entity.TotalTickets;
        entity.TotalTickets = dto.TotalTickets;
        entity.AvailableTickets += diff;
        
        entity.CategoryId = dto.CategoryId;
        
        return entity;
    }

    public static List<EventDto> ListEntityToListDto(List<Event> entities)
    {
        return entities.Select(e => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Date = e.Date,
            Location = e.Location,
            Price = e.Price,
            TotalTickets = e.TotalTickets,
            AvailableTickets = e.AvailableTickets,
            CategoryId = e.CategoryId,
            CategoryName = e.Category != null ? e.Category.Name : string.Empty
        }).ToList();
    }
}
