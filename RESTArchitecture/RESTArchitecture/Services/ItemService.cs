using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RESTArchitecture.Models;
using RESTArchitecture.Models.Items;

namespace RESTArchitecture.Services
{
    public class ItemService : IItemService
    {
        private readonly IMapper _mapper;
        private readonly RestDbContext _context;

        public ItemService(RestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<Item> CreateItem(ItemForCreate item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateItem();

            async Task<Item> CreateItem()
            {
                var createdItem = _mapper.Map<Item>(item);
                _context.Items.Add(createdItem);
                await _context.SaveChangesAsync();
                return createdItem;
            }
        }

        public async Task<Item> DeleteItem(int id)
        {
            var item = await GetItemById(id);

            if (item is null)
            {
                throw new ResourceNotFoundException("item does not exists");
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<IEnumerable<Item>> GetAllItems(ItemQuery itemQuery)
        {
            IQueryable<Item> items = _context.Items;

            if (itemQuery.CategoryId != null)
            {
                items = items.Where(i => i.CategoryId == itemQuery.CategoryId);
            }

            items = items
                .Skip(itemQuery.Size * (itemQuery.Page * 1))
                .Take(itemQuery.Size);
            return await items.ToArrayAsync();
        }

        public async Task<Item> GetItemById(int id)
        {
            var item = await _context.Items.Where(i => i.Id == id).FirstOrDefaultAsync();

            if (item == null)
            {
                throw new ResourceNotFoundException("item does not exists");
            }

            return item;
        }

        public async Task<Item> GetItemByIdWithCategory(int id)
        {
            var item = await _context.Items.Where(i => i.Id == id).Include(i => i.Category).FirstOrDefaultAsync();

            if (item == null)
            {
                throw new ResourceNotFoundException("item does not exists");
            }

            return item;
        }

        public Task UpdateItem(int id, Item item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateItem();

            async Task UpdateItem()
            {
                try
                {
                    _context.Entry(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Items.Any(i => i.Id == id))
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
