using NpgsqlTypes;

namespace WannaEat.Infrastructure.Persistence.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
}