namespace ticket_selling_backend.Entities;

public class Ticket : BaseEntity
{
    public int EventId { get; set; }
    public Event? Event { get; set; }
    public string UserId { get; set; } = string.Empty;
    public UserEntity? User { get; set; }
    public DateTime PurchaseDate { get; set; }
}
