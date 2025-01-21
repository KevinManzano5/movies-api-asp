using System.ComponentModel.DataAnnotations.Schema;

namespace Movies_API.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public string PathImg { get; set; }

        public enum Classification
        {
            Seven, Thirteen, Sixteen, Eigthteen
        }

        public Classification classification { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }
    }
}
