using IDistributedCacheRedis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace IDistributedCacheRedis.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            _distributedCache.SetString("name", "Teymur",new DistributedCacheEntryOptions() { AbsoluteExpiration=DateTime.Now.AddSeconds(15)});

            Product p = new() { Id = 1, Name = "Product1", Price = 14 };

            string jsonP=JsonConvert.SerializeObject(p);

            _distributedCache.SetString("Product:1", jsonP);


            var stproduct= _distributedCache.GetString("Product:1");
            Product product= JsonConvert.DeserializeObject<Product>(stproduct);



            return View(product);
        }
    }
}
