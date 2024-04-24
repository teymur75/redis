using RedisAppApi.Models;

namespace RedisAppApi.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetallProductsAsync();
        Task<Product> GetById(int id);
        Task<Product> CreateAsync(Product product);
    }
}
