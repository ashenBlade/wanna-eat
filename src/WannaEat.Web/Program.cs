using Microsoft.EntityFrameworkCore;
using WannaEat.Web;
using WannaEat.Web.Interfaces;
using WannaEat.Web.Services;
using WannaEat.Web.Services.RecipeServices;

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
        
        var uri = new UriBuilder(builder.Configuration.GetValue<string>("DATABASE_URL")).Uri;
        var userInfo = uri.UserInfo.Split(':');
        var (user, password) = ( userInfo[0], userInfo[1] );
        return
            $"Host={uri.Host};"
          + $"Database={uri.AbsolutePath};"
          + $"Port={uri.Port};"
          + $"User Id={user};"
          + $"Password={password}";

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