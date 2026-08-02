namespace ticket_selling_backend.DTOs.Orders;

public class CheckoutRequestDto
{
    public List<CheckoutItemDto> Items { get; set; } = new();
}

public class CheckoutItemDto
{
    public int EventId { get; set; }
    public int Quantity { get; set; }
}
