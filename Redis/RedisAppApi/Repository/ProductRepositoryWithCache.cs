using CacheServiceLibrary;
using RedisAppApi.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisAppApi.Repository
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private const string ProductKey = "ProductCaches";

        private readonly IProductRepository _repository;
        private readonly RedisService _redisService;
        private readonly IDatabase _memorydatabase;
        public ProductRepositoryWithCache(IProductRepository repository, RedisService redisService, IDatabase database)
        {
            _repository = repository;
            _redisService = redisService;
            _memorydatabase = database;
        }


        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct=await _repository.CreateAsync(product);

            if( await _memorydatabase.KeyExistsAsync(ProductKey) )
            {
                await _memorydatabase.HashSetAsync(ProductKey,newProduct.Id,JsonSerializer.Serialize(newProduct));
            }
            return newProduct;
        }

        public async Task<List<Product>> GetallProductsAsync()
        {
            if(! await _memorydatabase.KeyExistsAsync(ProductKey))
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();

            var cacheProducts=await _memorydatabase.HashGetAllAsync(ProductKey);

            foreach (var data in cacheProducts.ToList())
            {
                var product=JsonSerializer.Deserialize<Product>(data.Value);
                products.Add(product);
            }
            return products;
        }

        public async Task<Product> GetById(int id)
        {
            if( await _memorydatabase.KeyExistsAsync(ProductKey))
            {
                var product=await _memorydatabase.HashGetAsync(ProductKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : throw new ArgumentNullException();
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }


        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {

            var products =await _repository.GetallProductsAsync();

            products.ForEach(p =>
            {
                _memorydatabase.HashSetAsync(ProductKey,p.Id,JsonSerializer.Serialize(p));

            });
            return products;

        }
    }
}
