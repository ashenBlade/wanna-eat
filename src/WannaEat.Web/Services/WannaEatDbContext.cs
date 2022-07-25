using Microsoft.EntityFrameworkCore;
using WannaEat.Web.Models;

namespace WannaEat.Web.Services;

public class WannaEatDbContext: DbContext
{
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<DishProduct> DishProducts => Set<DishProduct>();
    public DbSet<CookingAppliance> CookingAppliances => Set<CookingAppliance>();
    public DbSet<>
    public WannaEatDbContext(DbContextOptions<WannaEatDbContext> options)
     : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        
        model.Entity<Dish>()
             .HasMany(d => d.RequiredToCook)
             .WithMany(c => c.UsedInCooking);
        
        model.Entity<DishProduct>(e =>
        {
            e.HasKey(dp => new{dp.DishId, dp.ProductId});
            e.HasOne(dp => dp.Dish)
             .WithMany(d => d.ConsistsOf)
             .HasForeignKey(dp => dp.DishId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(dp => dp.Product)
             .WithMany(p => p.RequiredForDish)
             .HasForeignKey(dp => dp.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}