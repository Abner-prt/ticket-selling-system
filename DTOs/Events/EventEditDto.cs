using System.ComponentModel.DataAnnotations;

namespace ticket_selling_backend.Dtos.Events;

public class EventEditDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string Location { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int TotalTickets { get; set; }
    [Required]
    public int CategoryId { get; set; }
}
