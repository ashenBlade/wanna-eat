using WannaEat.Domain.Entities;

namespace WannaEat.Domain.Interfaces;

public interface IIngredientRepository
{
    Ingredient? GetById(int id);
    IEnumerable<Ingredient> GetPaged(int page, int size);
    IEnumerable<Ingredient> SearchByName(string name);
}