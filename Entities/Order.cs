namespace ticket_selling_backend.Entities;

public class Order : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public UserEntity? User { get; set; }
    
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "Pending"; // Pending, Completed, Failed
    public string? StripeSessionId { get; set; }
    
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    
    public int EventId { get; set; }
    public Event? Event { get; set; }
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
