using System.ComponentModel.DataAnnotations;

namespace Movies_API.Models.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Max lenght 100")]
        public string Name { get; set; }
    }
}
