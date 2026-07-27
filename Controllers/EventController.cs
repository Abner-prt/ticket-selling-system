using Microsoft.AspNetCore.Mvc;
using ticket_selling_backend.Dtos.Events;
using ticket_selling_backend.Services.Events;

namespace ticket_selling_backend.Controllers;

[Route("api/event")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult> GetPage(string searchTerm = "", int page = 1, int pageSize = 10)
    {
        var response = await _eventService.GetPageAsync(searchTerm, page, pageSize);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetOne(int id)
    {
        var result = await _eventService.GetOneByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult> Create(EventCreateDto dto)
    {
        var result = await _eventService.CreateAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, EventEditDto dto)
    {
        var result = await _eventService.EditAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _eventService.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
