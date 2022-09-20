using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WannaEat.Infrastructure.Persistence.Models;

namespace WannaEat.Infrastructure.Persistence;

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
            ingredient.HasIndex(i => i.Name)
                      .HasSortOrder(SortOrder.Ascending)
                      .HasNullSortOrder(NullSortOrder.NullsLast);
        });
    }
}