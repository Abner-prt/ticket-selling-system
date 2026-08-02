using ticket_selling_backend.Dtos.Common;
using ticket_selling_backend.Dtos.Events;

namespace ticket_selling_backend.Services.Events;

public interface IEventService
{
    Task<ResponseDto<PageDto<List<EventDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10);
    Task<ResponseDto<EventDto>> GetOneByIdAsync(int id);
    Task<ResponseDto<EventDto>> CreateAsync(EventCreateDto dto);
    Task<ResponseDto<EventDto>> EditAsync(int id, EventEditDto dto);
    Task<ResponseDto<EventDto>> DeleteAsync(int id);
}
