using ticket_selling_backend.Dtos.Categories;
using ticket_selling_backend.Dtos.Common;

namespace ticket_selling_backend.Services.Categories;

public interface ICategoryService
{
    Task<ResponseDto<PageDto<List<CategoryDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10);
    Task<ResponseDto<CategoryDto>> GetOneByIdAsync(int id);
    Task<ResponseDto<CategoryDto>> CreateAsync(CategoryCreateDto dto);
    Task<ResponseDto<CategoryDto>> EditAsync(int id, CategoryEditDto dto);
    Task<ResponseDto<CategoryDto>> DeleteAsync(int id);
}
