using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_selling_backend.Data;
using ticket_selling_backend.DTOs;
using ticket_selling_backend.Mappers;

namespace ticket_selling_backend.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EventController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _context.Events.ToListAsync();
        var dtos = events.Select(e => e.ToDto()).ToList();
        return Ok(ResponseDto<object>.Success(dtos, "Eventos obtenidos exitosamente"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var evt = await _context.Events.FindAsync(id);
        if (evt == null)
            return NotFound(ResponseDto<object>.Failure("Evento no encontrado", 404));

        return Ok(ResponseDto<EventDto>.Success(evt.ToDto(), "Evento obtenido"));
    }
}
