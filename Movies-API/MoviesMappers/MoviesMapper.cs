using AutoMapper;
using Movies_API.Models;
using Movies_API.Models.Dtos;

namespace Movies_API.MoviesMapper
{
    public class MoviesMapper : Profile
    {
        public MoviesMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
        }
    }
}
