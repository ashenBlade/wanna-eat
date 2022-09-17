import {Ingredient} from "../entities/ingredient";
import {Recipe} from "../entities/recipe";

export interface IRecipeService {
    findRelevantRecipes(products: Ingredient[]): Promise<Recipe[]>
}