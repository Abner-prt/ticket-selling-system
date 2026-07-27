using System.Net;
using Microsoft.EntityFrameworkCore;
using ticket_selling_backend.Constants;
using ticket_selling_backend.Data;
using ticket_selling_backend.Dtos.Categories;
using ticket_selling_backend.Dtos.Common;
using ticket_selling_backend.Entities;
using ticket_selling_backend.Mappers;

namespace ticket_selling_backend.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly int PAGE_SIZE;
    private readonly int PAGE_SIZE_LIMIT;

    public CategoryService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        PAGE_SIZE = configuration.GetValue<int>("PageSize");
        PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
    }

    public async Task<ResponseDto<PageDto<List<CategoryDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10)
    {
        page = Math.Abs(page);
        pageSize = Math.Abs(pageSize);

        pageSize = pageSize <= 0 ? PAGE_SIZE : pageSize;
        pageSize = pageSize > PAGE_SIZE_LIMIT ? PAGE_SIZE_LIMIT : pageSize;

        int startIndex = (page - 1) * pageSize;

        IQueryable<Category> query = _context.Categories;
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
        }

        int totalRows = await query.CountAsync();

        var entities = await query
            .OrderBy(c => c.Name)
            .Skip(startIndex)
            .Take(pageSize)
            .ToListAsync();

        return new ResponseDto<PageDto<List<CategoryDto>>>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTERS_FOUND,
            Data = new PageDto<List<CategoryDto>>
            {
                CurrentPage = page == 0 ? 1 : page,
                PageSize = pageSize,
                TotalItems = totalRows,
                TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                Items = CategoryMapper.ListEntityToListDto(entities),
                HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT && 
                    page < (int)Math.Ceiling((double)totalRows / pageSize),
                HasPreviousPage = page > 1
            }
        };
    }

    public async Task<ResponseDto<CategoryDto>> GetOneByIdAsync(int id)
    {
        var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
        {
            return new ResponseDto<CategoryDto>
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                Status = false,
            };
        }

        return new ResponseDto<CategoryDto>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = HttpMessageResponse.REGISTER_FOUND,
            Status = true,
            Data = new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name
            }
        };
    }

    public async Task<ResponseDto<CategoryDto>> CreateAsync(CategoryCreateDto dto)
    {
        Category entity = CategoryMapper.CreateDtoToEntity(dto);

        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();

        return new ResponseDto<CategoryDto>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = HttpMessageResponse.REGISTER_CREATED,
            Status = true,
            Data = new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name
            }
        };
    }

    public async Task<ResponseDto<CategoryDto>> EditAsync(int id, CategoryEditDto dto)
    {
        var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
        {
            return new ResponseDto<CategoryDto>
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND,
            };
        }

        var updatedEntity = CategoryMapper.EditDtoToEntity(entity, dto);
        _context.Categories.Update(updatedEntity);
        await _context.SaveChangesAsync();

        return new ResponseDto<CategoryDto>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_UPDATED,
            Data = new CategoryDto
            {
                Id = updatedEntity.Id,
                Name = updatedEntity.Name
            }
        };
    }

    public async Task<ResponseDto<CategoryDto>> DeleteAsync(int id)
    {
        var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
        {
            return new ResponseDto<CategoryDto>
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND,
            };
        }

        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync();

        return new ResponseDto<CategoryDto>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_DELETED,
            Data = new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name
            }
        };
    }
}
