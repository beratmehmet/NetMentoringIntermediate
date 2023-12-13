using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTArchitecture.Models;
using RESTArchitecture.Models.Items;
using RESTArchitecture.Services;

namespace RESTArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemsController(RestDbContext context, IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllItems([FromQuery] ItemQuery itemQuery)
        {
            return Ok(await _itemService.GetAllItems(itemQuery));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetItemById(int id)
        {
            var item = await _itemService.GetItemById(id);

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(ItemForCreate item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdItem = await _itemService.CreateItem(item);

            return CreatedAtAction(
                nameof(GetItemById),
                new { id = createdItem.Id },
                createdItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, [FromBody] Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            await _itemService.UpdateItem(id, item);

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await _itemService.DeleteItem(id);

            return item;
        }
    }
}
