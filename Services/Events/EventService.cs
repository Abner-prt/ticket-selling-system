using System.Net;
using Microsoft.EntityFrameworkCore;
using ticket_selling_backend.Constants;
using ticket_selling_backend.Data;
using ticket_selling_backend.Dtos.Common;
using ticket_selling_backend.Dtos.Events;
using ticket_selling_backend.Entities;
using ticket_selling_backend.Mappers;

namespace ticket_selling_backend.Services.Events;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;
    private readonly int PAGE_SIZE;
    private readonly int PAGE_SIZE_LIMIT;

    public EventService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        PAGE_SIZE = configuration.GetValue<int>("PageSize");
        PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
    }

    public async Task<ResponseDto<PageDto<List<EventDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10)
    {
        page = Math.Abs(page);
        pageSize = Math.Abs(pageSize);

        pageSize = pageSize <= 0 ? PAGE_SIZE : pageSize;
        pageSize = pageSize > PAGE_SIZE_LIMIT ? PAGE_SIZE_LIMIT : pageSize;

        int startIndex = (page - 1) * pageSize;

        IQueryable<Event> query = _context.Events.Include(e => e.Category);
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e => e.Title.ToLower().Contains(searchTerm.ToLower()) || e.Location.ToLower().Contains(searchTerm.ToLower()));
        }

        int totalRows = await query.CountAsync();

        var entities = await query
            .OrderBy(e => e.Title)
            .Skip(startIndex)
            .Take(pageSize)
            .ToListAsync();

        return new ResponseDto<PageDto<List<EventDto>>>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTERS_FOUND,
            Data = new PageDto<List<EventDto>>
            {
                CurrentPage = page == 0 ? 1 : page,
                PageSize = pageSize,
                TotalItems = totalRows,
                TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                Items = EventMapper.ListEntityToListDto(entities),
                HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT && 
                    page < (int)Math.Ceiling((double)totalRows / pageSize),
                HasPreviousPage = page > 1
            }
        };
    }

    public async Task<ResponseDto<EventDto>> GetOneByIdAsync(int id)
    {
        var entity = await _context.Events
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (entity is null)
        {
            return new ResponseDto<EventDto>
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                Status = false,
            };
        }

        return new ResponseDto<EventDto>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = HttpMessageResponse.REGISTER_FOUND,
            Status = true,
            Data = EventMapper.ListEntityToListDto(new List<Event>{ entity }).First()
        };
    }

    public async Task<ResponseDto<EventDto>> CreateAsync(EventCreateDto dto)
    {
        Event entity = EventMapper.CreateDtoToEntity(dto);

        _context.Events.Add(entity);
        await _context.SaveChangesAsync();

        entity = await _context.Events.Include(e => e.Category).FirstAsync(e => e.Id == entity.Id);

        return new ResponseDto<EventDto>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = HttpMessageResponse.REGISTER_CREATED,
            Status = true,
            Data = EventMapper.ListEntityToListDto(new List<Event>{ entity }).First()
        };
    }

    public async Task<ResponseDto<EventDto>> EditAsync(int id, EventEditDto dto)
    {
        var entity = await _context.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id);

        if (entity is null)
        {
            return new ResponseDto<EventDto>
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND,
            };
        }

        var updatedEntity = EventMapper.EditDtoToEntity(entity, dto);
        _context.Events.Update(updatedEntity);
        await _context.SaveChangesAsync();

        updatedEntity = await _context.Events.Include(e => e.Category).FirstAsync(e => e.Id == updatedEntity.Id);

        return new ResponseDto<EventDto>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_UPDATED,
            Data = EventMapper.ListEntityToListDto(new List<Event>{ updatedEntity }).First()
        };
    }

    public async Task<ResponseDto<EventDto>> DeleteAsync(int id)
    {
        var entity = await _context.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id);

        if (entity is null)
        {
            return new ResponseDto<EventDto>
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND,
            };
        }

        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();

        return new ResponseDto<EventDto>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_DELETED,
            Data = EventMapper.ListEntityToListDto(new List<Event>{ entity }).First()
        };
    }
}
