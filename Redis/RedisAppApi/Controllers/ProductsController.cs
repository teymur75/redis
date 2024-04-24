using CacheServiceLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisAppApi.Models;
using RedisAppApi.Repository;
using StackExchange.Redis;

namespace RedisAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _productRepo;
        private readonly IDatabase _database;

        public ProductsController(IProductRepository productRepo, StackExchange.Redis.IDatabase database)
        {
            _productRepo = productRepo;
            _database = database;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepo.GetallProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            return Ok(await _productRepo.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(String.Empty, await _productRepo.CreateAsync(product));
        }


    }
}
