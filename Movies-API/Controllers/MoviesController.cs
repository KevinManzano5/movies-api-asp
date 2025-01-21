using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies_API.Models;
using Movies_API.Models.Dtos;
using Movies_API.Repository.IRepository;

namespace Movies_API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _repo;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetMovies()
        {
            var movies = _repo.GetMovies();

            var moviesListDto = new List<MovieDto>();

            foreach (var movie in movies)
            {
                moviesListDto.Add(_mapper.Map<MovieDto>(movie));
            }

            return Ok(moviesListDto);
        }

        [HttpGet("{id:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int id)
        {
            var movie = _repo.GetMovie(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<CategoryDto>(movie);

            return Ok(movieDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createMovieDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.MovieExists(createMovieDto.Name))
            {
                ModelState.AddModelError("", $"Category already exists");

                return StatusCode(400, ModelState);
            }

            var movie = _mapper.Map<Movie>(createMovieDto);

            if (!_repo.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Error while saving record: {movie.Name}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetMovie", new { id = movie.Id }, movie);
        }

        [HttpPatch("{id:int}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovie(int id, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (movieDto == null || id != movieDto.Id)
            {
                return BadRequest(ModelState);
            }

            var movieExists = _repo.GetMovie(id);

            if (movieExists == null)
            {
                return NotFound($"Category with id {id} not found");
            }

            var movie = _mapper.Map<Movie>(movieDto);

            if (!_repo.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Error while saving record: {movie.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(int id)
        {
            if (!_repo.MovieExists(id))
            {
                return NotFound();
            }

            var movie = _repo.GetMovie(id);

            if (!_repo.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Error while saving record: {movie.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("category/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMoviesByCategory(int categoryId)
        {
            var moviesList = _repo.GetMoviesByCategory(categoryId);

            if (moviesList == null)
            {
                return NotFound();
            }

            var movies = new List<MovieDto>();

            foreach (var movie in moviesList)
            {
                movies.Add(_mapper.Map<MovieDto>(movie));
            }

            return Ok(movies);
        }

        [HttpGet("find")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Find(string name)
        {
            try
            {
                var result = _repo.FindMovie(name);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in the search");
            }
        }
    }
}
