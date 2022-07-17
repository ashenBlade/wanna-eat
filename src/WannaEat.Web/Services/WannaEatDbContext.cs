using Microsoft.EntityFrameworkCore;
using WannaEat.Web.Models;

namespace WannaEat.Web.Services;

public class WannaEatDbContext: DbContext
{
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Product> Products => Set<Product>();

    public WannaEatDbContext(DbContextOptions<WannaEatDbContext> options)
    :base(options)
    { }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        model.Entity<Dish>()
             .HasMany(d => d.RequiredToCook)
             .WithMany(c => c.UsedInCooking);
        
    }
}