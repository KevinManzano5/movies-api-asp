using System.ComponentModel.DataAnnotations.Schema;

namespace Movies_API.Models.Dtos
{
    public class CreateMovieDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public string PathImg { get; set; }

        public enum CreateClassification
        {
            Seven, Thirteen, Sixteen, Eigthteen
        }

        public CreateClassification classification { get; set; }

        public int CategoryId { get; set; }
    }
}
