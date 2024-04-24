using Microsoft.EntityFrameworkCore;
using RedisAppApi.Context;
using RedisAppApi.Models;

namespace RedisAppApi.Repository
{


    public class ProductRepository : IProductRepository
    {

        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetallProductsAsync()
        {
            return await _appDbContext.Products.ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _appDbContext.Products.FindAsync(id);
        }
    }
}
