using Microsoft.EntityFrameworkCore;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Services;

namespace WannaEat.Infrastructure.Persistence;

public class PostgresqlIngredientsRepository: IIngredientRepository 
{
    private readonly WannaEatDbContext _context;

    public PostgresqlIngredientsRepository(WannaEatDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Ingredient>> FindByNameAsync(string? name, int page, int size, CancellationToken token = default)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page number must be positive");
        }

        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Page size must be positive");
        }
        
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        name = name.ToLower();
        var dbIngredients =  await _context.Ingredients
                                           .Where(i => i.Name
                                                        .ToLower()
                                                        .Contains(name))
                                           .OrderBy(i => i.Name)
                                           .Skip(( page - 1 ) * size)
                                           .Take(size)
                                           .ToListAsync(token);
        return dbIngredients
              .Select(i => new Ingredient(i.Id, i.Name))
              .ToList();
    }

    public async Task<List<Ingredient>> FindAllByIdAsync(int[] ids, CancellationToken token = default)
    {
        if (ids is null)
        {
            throw new ArgumentNullException(nameof(ids));
        }

        var ingredients = await _context.Ingredients
                                        .Where(i => ids.Contains(i.Id))
                                        .ToListAsync(token);
        return ingredients
              .Select(i => new Ingredient(i.Id, i.Name))
              .ToList();
    }

    public async Task<Ingredient?> FindByIdAsync(int id, CancellationToken token = default)
    {
        var dbIngredient = await _context.Ingredients
                                         .SingleOrDefaultAsync(i => i.Id == id, token);
        
        return dbIngredient is null 
                   ? null 
                   : new Ingredient(dbIngredient.Id, dbIngredient.Name);
    }

    public async Task<List<Ingredient>> GetIngredientsPaged(int page, int size, CancellationToken token = default)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page number must be positive");
        }

        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Page size must be positive");
        }
        
        var dbIngredients = await _context.Ingredients
                                          .OrderBy(i => i.Id)
                                          .Skip(( page - 1 ) * size)
                                          .Take(size)
                                          .ToListAsync(token);
        return dbIngredients
              .Select(i => new Ingredient(i.Id, i.Name))
              .ToList();
    }
}