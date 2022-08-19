using Microsoft.EntityFrameworkCore;
using WannaEat.Domain.Interfaces;
using WannaEat.FoodService.MZR;
using WannaEat.Infrastructure.Persistence;
using WannaEat.Web;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.IsHeroku())
{
    builder.WebHost.ConfigureKestrel(kestrel =>
    {
        kestrel.ListenAnyIP(builder.Configuration.GetValue<int>("PORT"));
    });
}

builder.Services
       .AddControllers()
       .AddNewtonsoftJson();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IRecipeService, MZRRecipeService>();
builder.Services.AddDbContext<WannaEatDbContext>(db =>
{
    string GetConnectionString()
    {
        if (!builder.Configuration.IsHeroku())
            return builder.Configuration.GetConnectionString("WannaEat");
        var uri = new UriBuilder(builder.Configuration.GetValue<string>("DATABASE_URL"));
        return $"Host={uri.Host};Database={uri.Path.Trim('/')};Port={uri.Port};User Id={uri.UserName};Password={uri.Password}";
    }

    db.UseNpgsql(GetConnectionString());
    
    if (builder.Environment.IsProduction()) 
        return;
    
    db.EnableSensitiveDataLogging();
    db.EnableDetailedErrors();
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
app.MapControllerRoute("default", "{controller}/{action}");

app.MapFallbackToFile("index.html");

app.Run();