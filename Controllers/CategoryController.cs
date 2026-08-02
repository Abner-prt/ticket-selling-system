using Microsoft.AspNetCore.Mvc;
using ticket_selling_backend.Dtos.Categories;
using ticket_selling_backend.Services.Categories;

namespace ticket_selling_backend.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult> GetPage(string searchTerm = "", int page = 1, int pageSize = 10)
    {
        var response = await _categoryService.GetPageAsync(searchTerm, page, pageSize);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetOne(int id)
    {
        var result = await _categoryService.GetOneByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CategoryCreateDto dto)
    {
        var result = await _categoryService.CreateAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CategoryEditDto dto)
    {
        var result = await _categoryService.EditAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
