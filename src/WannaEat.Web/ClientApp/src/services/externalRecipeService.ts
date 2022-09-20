import {Recipe} from "../entities/recipe";
import {Ingredient} from "../entities/ingredient";
import {IRecipeService} from "../interfaces/IRecipeService";

export class ExternalRecipeService implements IRecipeService {
    async findRelevantRecipes(products: Ingredient[]): Promise<Recipe[]> {
        const max = 40;
        const productsQuery = products.map(p => `contain=${p.id}`).join('&')
        return await fetch(`/api/v1/recipes/search?max=${max}&${productsQuery}`)
            .then(res => res.json())
    }
}