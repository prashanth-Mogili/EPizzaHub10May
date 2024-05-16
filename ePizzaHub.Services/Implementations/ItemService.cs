using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Services.Implementations
{
    public class ItemService : Service<Item>, IItemService
    {
        IRepository<Item> _itemRepo;
        public ItemService(IRepository<Item> itemRepo):base(itemRepo) 
        { 
            _itemRepo = itemRepo;
        }
      
        public IEnumerable<ItemModel> GetItems()
        {
            var data = _itemRepo.GetAll().Select(i => new ItemModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                UnitPrice = i.UnitPrice,
                ImageUrl = i.ImageUrl
            });
            return data;
        }
    }
}
