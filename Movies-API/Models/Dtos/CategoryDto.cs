using System.ComponentModel.DataAnnotations;

namespace Movies_API.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Max lenght 100")]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
