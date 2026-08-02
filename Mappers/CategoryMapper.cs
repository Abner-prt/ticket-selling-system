using ticket_selling_backend.Dtos.Categories;
using ticket_selling_backend.Entities;

namespace ticket_selling_backend.Mappers;

public static class CategoryMapper
{
    public static Category CreateDtoToEntity(CategoryCreateDto dto)
    {
        return new Category
        {
            Name = dto.Name
        };
    }

    public static Category EditDtoToEntity(Category entity, CategoryEditDto dto)
    {
        entity.Name = dto.Name;
        return entity;
    }

    public static List<CategoryDto> ListEntityToListDto(List<Category> entities)
    {
        return entities.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    }
}
