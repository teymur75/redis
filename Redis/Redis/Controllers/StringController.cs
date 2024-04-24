using Microsoft.AspNetCore.Mvc;

namespace Redis.Controllers
{
    public class StringController : Controller
    {
        private readonly RedisService.RedisService _redisService;

        public StringController(RedisService.RedisService redisService)
        {
            _redisService = redisService;
        }

        public IActionResult Index()
        {
            var db= _redisService.GetDatabase(2);

            db.StringSet("class","P134");
            db.StringSet("class","P135");

            return View();
        }
    }
}
