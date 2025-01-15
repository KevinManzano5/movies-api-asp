using Movies_API.Models;

namespace Movies_API.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int CategoryId);

        bool CategoryExists(int CategoryId);

        bool CategoryExists(string Name);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);

        bool Save();
    }
}
