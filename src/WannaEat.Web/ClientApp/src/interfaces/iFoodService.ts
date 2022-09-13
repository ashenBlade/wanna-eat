import {Ingredient} from "../entities/ingredient";
import {Recipe} from "../entities/recipe";

export interface IFoodService {
    findRelevantDishes(products: Ingredient[]): Promise<Recipe[]>
}