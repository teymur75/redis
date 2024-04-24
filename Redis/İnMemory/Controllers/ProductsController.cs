using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace İnMemory.Controllers
{
    public class ProductsController : Controller
    {

        readonly IMemoryCache _memoryCache;

        public ProductsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            _memoryCache.Set<string>("time", DateTime.Now.ToString());
            var data=_memoryCache.Get<string>("time");
            ViewBag.Data=data;

            if(!_memoryCache.TryGetValue("time",out string time))
            {
                _memoryCache.Set<string>("time", DateTime.Now.ToString() ,new MemoryCacheEntryOptions() { AbsoluteExpiration=DateTime.Now.AddSeconds(30)} );
            }

            ViewBag.Data1 = time;

            return View();
        }
    }
}
