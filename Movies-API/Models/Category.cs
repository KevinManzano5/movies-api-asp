using System.ComponentModel.DataAnnotations;

namespace Movies_API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
