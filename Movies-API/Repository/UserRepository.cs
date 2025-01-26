using Microsoft.IdentityModel.Tokens;
using Movies_API.Data;
using Movies_API.Models;
using Movies_API.Models.Dtos;
using Movies_API.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Movies_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly string _secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _secretKey = configuration.GetValue<string>("ApiSettings:secretKey");
        }

        public User GetUser(int id)
        {
            return _db.User.FirstOrDefault(user => user.Id == id);
        }

        public ICollection<User> GetUsers()
        {
            return _db.User.OrderBy(user => user.Username).ToList();
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.User.FirstOrDefault(user => user.Username == username);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginUserResponseDto> Login(LoginUserDto loginUserDto)
        {
            var password = getMD5(loginUserDto.Password);

            var user = _db.User.FirstOrDefault(user => user.Username.ToLower() == loginUserDto.Username.ToLower() && user.Password == password);

            if (user == null)
            {
                return new LoginUserResponseDto()
                {
                    Token = "",
                    User = null,
                };

            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginUserResponseDto loginUserResponseDto = new LoginUserResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };

            return loginUserResponseDto;
        }

        public async Task<User> Register(CreateUserDto createUserDto)
        {
            var password = getMD5(createUserDto.Password);

            User user = new User()
            {
                Username = createUserDto.Username,
                Password = password,
                Name = createUserDto.Name,
                Role = createUserDto.Role
            };

            _db.User.Add(user);

            await _db.SaveChangesAsync();

            user.Password = password;

            return user;
        }

        private string getMD5(string password)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();

            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);

            data = x.ComputeHash(data);

            string resp = "";

            for (int i = 0; i < data.Length; i++) resp += data[i].ToString("x2").ToLower();

            return resp;
        }
    }
}
