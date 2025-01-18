using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies_API.Models;
using Movies_API.Models.Dtos;
using Movies_API.Repository.IRepository;

namespace Movies_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _ctRepo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategories()
        {
            var categories = _ctRepo.GetCategories();

            var categoriesListDto = new List<CategoryDto>();

            foreach (var category in categories)
            {
                categoriesListDto.Add(_mapper.Map<CategoryDto>(category));
            }

            return Ok(categoriesListDto);
        }

        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _ctRepo.GetCategory(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createCategoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ctRepo.CategoryExists(createCategoryDto.Name))
            {
                ModelState.AddModelError("", $"Category already exists");

                return StatusCode(400, ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);

            if (!_ctRepo.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Error while saving record: {category.Name}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
        }

        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryDto == null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_ctRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Error while saving record: {category.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_ctRepo.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var category = _ctRepo.GetCategory(categoryId);

            if (!_ctRepo.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Error while saving record: {category.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
