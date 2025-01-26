using Movies_API.Models;
using Movies_API.Models.Dtos;

namespace Movies_API.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();

        User GetUser(int id);

        bool IsUniqueUser(string username);

        Task<LoginUserResponseDto> Login(LoginUserDto loginUserDto);

        Task<User> Register(CreateUserDto createUserDto);
    }
}
