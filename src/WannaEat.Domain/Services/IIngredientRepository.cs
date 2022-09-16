using WannaEat.Domain.Entities;

namespace WannaEat.Domain.Services;

public interface IIngredientRepository
{
    Task<List<Ingredient>> FindByNameAsync(string? name, int page = 1, int size = 15, CancellationToken token = default);
    Task<Ingredient?> FindByIdAsync(int id, CancellationToken token = default);
    Task<List<Ingredient>> GetIngredientsPaged(int page, int size, CancellationToken token = default);
}