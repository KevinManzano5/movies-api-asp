using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies_API.Models;
using Movies_API.Models.Dtos;
using Movies_API.Repository.IRepository;
using System.Net;

namespace Movies_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            this._apiResponse = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUsers()
        {
            var users = _repo.GetUsers();

            var usersListDto = new List<UserDto>();

            foreach (var user in users)
            {
                usersListDto.Add(_mapper.Map<UserDto>(user));
            }

            return Ok(usersListDto);
        }

        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int userId)
        {
            var user = _repo.GetUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            bool isUnique = _repo.IsUniqueUser(createUserDto.Username);

            if (!isUnique)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage.Add("Username already exists");

                return BadRequest(_apiResponse);
            }

            var user = await _repo.Register(createUserDto);

            if (user == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage.Add("Username already exists");

                return BadRequest(_apiResponse);
            }

            _apiResponse.StatusCode = HttpStatusCode.Created;
            _apiResponse.IsSuccess = true;

            return Ok(user);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var response = await _repo.Login(loginUserDto);

            if (response.User == null || string.IsNullOrEmpty(response.Token))
            {
                _apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage.Add("Username or password incorrect");

                return BadRequest(_apiResponse);
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = response;

            return Ok(response);
        }
    }
}
