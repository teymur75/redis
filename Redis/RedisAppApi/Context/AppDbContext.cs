using Microsoft.EntityFrameworkCore;
using RedisAppApi.Models;

namespace RedisAppApi.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options ):base(options)
        {
            
        }

        public DbSet<Product>    Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(

                new Product() {Id=1,Name="Product1",Price=1 },
                new Product() {Id=2,Name="Product2",Price=2 },
                new Product() {Id=3,Name="Product3",Price=3 }
                );


            base.OnModelCreating(modelBuilder);
        }
    }
}
