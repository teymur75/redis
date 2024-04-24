using CacheServiceLibrary;
using Microsoft.EntityFrameworkCore;
using RedisAppApi.Context;
using RedisAppApi.Repository;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseInMemoryDatabase("x");
});

builder.Services.AddSingleton<RedisService>(serviceprovider =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});

builder.Services.AddSingleton<IDatabase>(sp =>
{
    var redisService=sp.GetRequiredService<RedisService>();
    var db=redisService.GetDb(0);
    return db;
});


builder.Services.AddScoped<IProductRepository>(sp =>
{
    var context=sp.GetRequiredService<AppDbContext>(); 
    var productRepository = new ProductRepository(context);
    var redisService = sp.GetRequiredService<RedisService>();
    var memoryDb = redisService.GetDb(0);

    return new ProductRepositoryWithCache(productRepository,redisService,memoryDb);
});

var app = builder.Build();


using(var scope = app.Services.CreateScope())
{
    var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
