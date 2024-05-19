using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace ePizzaHub.UI.Controllers
{
    public class HomeController : Controller
    {
        IItemService _itemService;
        IMemoryCache _memoryCache;


        public HomeController(IItemService itemService, IMemoryCache memoryCache)
        {
           _itemService = itemService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            // var items = _itemService.GetItems();
            var items = _memoryCache.GetOrCreate("Items", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return _itemService.GetItems();
            });
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
