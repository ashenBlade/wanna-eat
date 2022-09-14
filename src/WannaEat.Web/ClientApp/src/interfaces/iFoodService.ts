import {Ingredient} from "../entities/ingredient";
import {Recipe} from "../entities/recipe";

export interface IFoodService {
    findRelevantRecipes(products: Ingredient[]): Promise<Recipe[]>
}