using System.ComponentModel.DataAnnotations;

namespace ticket_selling_backend.Dtos.Categories;

public class CategoryEditDto
{
    [Required(ErrorMessage = "El nombre de la categoría es requerido.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Name { get; set; } = string.Empty;
}
