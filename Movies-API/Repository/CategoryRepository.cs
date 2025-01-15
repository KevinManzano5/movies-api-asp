using Movies_API.Data;
using Movies_API.Models;
using Movies_API.Repository.IRepository;

namespace Movies_API.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CategoryExists(int CategoryId)
        {
            return _db.Category.Any(category => category.Id == CategoryId);
        }

        public bool CategoryExists(string Name)
        {
            bool value = _db.Category.Any(category => category.Name.ToLower().Trim() == Name.ToLower().Trim());

            return value;
        }

        public bool CreateCategory(Category category)
        {
            category.CreatedAt = DateTime.Now;

            _db.Category.Add(category);

            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Category.Remove(category);

            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Category.OrderBy(category => category.Name).ToList();
        }

        public Category GetCategory(int CategoryId)
        {
            return _db.Category.FirstOrDefault(category => category.Id == CategoryId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            category.CreatedAt = DateTime.Now;

            _db.Category.Update(category);

            return true;
        }
    }
}
