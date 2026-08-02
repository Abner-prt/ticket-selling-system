using ticket_selling_backend.DTOs;
using ticket_selling_backend.DTOs.Orders;

namespace ticket_selling_backend.Services.Orders;

public interface IOrderService
{
    Task<ResponseDto<object>> CheckoutAsync(string userId, CheckoutRequestDto dto);
    Task<ResponseDto<IEnumerable<object>>> GetMyOrdersAsync(string userId);
}
