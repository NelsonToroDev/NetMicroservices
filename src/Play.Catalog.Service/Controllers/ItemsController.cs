using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDto> itemDtos = new()
    {
        new ItemDto(Guid.NewGuid(), "Poison", "Reduce the amount of HP", 5, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Bronce sword", "Delas a small ampunt of damage", 20, DateTimeOffset.UtcNow)
    };

    // GET /items
    [HttpGet]
    public IEnumerable<ItemDto> Get()
    {
        return itemDtos;
    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public ItemDto? GetById(Guid id)
    {
        return itemDtos.SingleOrDefault(i => i.Id == id);
    }

    // POST /items/{id}
    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
    {
        var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
        itemDtos.Add(item);
        // tells where to find the created object, in this case calling to GetById method with the specified id parameter
        // https://localhost:7032/items/bb537d2f-6ae2-4b4c-931c-3ff0d7217be9 
        // http 201 created at
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);

    }

    // PUT /items/{id}
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem = itemDtos.Where(i => i.Id == id).SingleOrDefault();
        if (existingItem == null)
        {
            return this.NotFound($"ItemDto with id '{id}' not found");
        }

        var updatedItem = existingItem with
        {
            Name = updateItemDto.Name,
            Description = updateItemDto.Description,
            Price = updateItemDto.Price
        };

        var index = itemDtos.FindIndex(i => i.Id == id);
        itemDtos[index] = updatedItem;
        // http 204 No Content
        return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var index = itemDtos.FindIndex(i => i.Id == id);
        itemDtos.RemoveAt(index);
        return NoContent();
    }
}
