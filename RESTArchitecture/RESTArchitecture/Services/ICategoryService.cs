using RESTArchitecture.Models.Categories;

namespace RESTArchitecture.Services
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(CategoryForCreate category);
        Task<Category> DeleteCategory(int id);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> GetCategoryWitItems(int id);
        Task UpdateCategory(int id, Category category);


    }
}
