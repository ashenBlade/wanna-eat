using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using WannaEat.Web.Models;

namespace WannaEat.Web.Services;

public class WannaEatDbContext: DbContext
{
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    
    public WannaEatDbContext(DbContextOptions<WannaEatDbContext> options)
     : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>(ingredient =>
        {
            ingredient.HasGeneratedTsVectorColumn(i => i.NameSearchVector,
                                                  "russian",
                                                  i => new {i.Name})
                      .HasIndex(i => i.NameSearchVector)
                      .HasMethod("GIN");
        });
    }
}