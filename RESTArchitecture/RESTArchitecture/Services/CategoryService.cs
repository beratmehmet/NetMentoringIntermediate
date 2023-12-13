using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RESTArchitecture.Models;
using RESTArchitecture.Models.Categories;

namespace RESTArchitecture.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly RestDbContext _context;

        public CategoryService(RestDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<Category> CreateCategory(CategoryForCreate category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            return CreateCategory();

            async Task<Category> CreateCategory()
            {
                var categoryForCreate = _mapper.Map<Category>(category);
                _context.Categories.Add(categoryForCreate);
                await _context.SaveChangesAsync();
                return categoryForCreate;
            }
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var category = await GetCategoryWitItems(id);

            if (category is null)
            {
                throw new ResourceNotFoundException("category does not exists");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
           return await _context.Categories.ToArrayAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                throw new ResourceNotFoundException("category does not exists");
            }

            return category;
        }

        public async Task<Category> GetCategoryWitItems(int id)
        {
            var category = await _context.Categories.Where(c => c.Id == id).Include(c => c.Items).FirstOrDefaultAsync();

            if (category == null)
            {
                throw new ResourceNotFoundException("category does not exists");
            }

            return category;
        }

        public Task UpdateCategory(int id, Category category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            return UpdateCategory();

            async Task UpdateCategory()
            {
                try
                {
                    _context.Entry(category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(i => i.Id == id))
                    {
                        throw new ResourceNotFoundException("item does not exists");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
