using Microsoft.EntityFrameworkCore;

using MusicAPI;
using MusicAPI.Repositories;
using MusicAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionStr = builder.Configuration.GetSection("ConnectionStrings");

// Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionStr["DefaultConnection"]));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVinylRepository, VinylRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();  // База создается один раз при старте приложения
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();