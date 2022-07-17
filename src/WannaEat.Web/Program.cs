using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WannaEat.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<WannaEatDbContext>(db =>
{
    db.UseNpgsql(builder.Configuration.GetConnectionString("WannaEat"));
    if (builder.Environment.IsDevelopment())
    {
        db.EnableDetailedErrors();
        db.UseMemoryCache(new MemoryCache(new MemoryCacheOptions()));
    }
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();