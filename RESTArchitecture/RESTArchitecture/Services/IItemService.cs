using Microsoft.AspNetCore.Mvc;
using RESTArchitecture.Models.Items;

namespace RESTArchitecture.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItems(ItemQuery item);
        Task<Item> GetItemById(int id);
        Task<Item> GetItemByIdWithCategory(int id);
        Task<Item> CreateItem(ItemForCreate item);
        Task UpdateItem(int id, Item item);
        Task<Item> DeleteItem(int id);

    }
}
