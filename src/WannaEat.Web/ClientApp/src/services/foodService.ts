import {Recipe} from "../entities/recipe";
import {Ingredient} from "../entities/ingredient";

export interface IFoodService {
    findRelevantDishes(products: Ingredient[]): Promise<Recipe[]>
}

export class FoodService implements IFoodService {
    async findRelevantDishes(products: Ingredient[]): Promise<Recipe[]> {
        const productsQuery = products.map(p => `contain=${p.id}`).join('&')
        return await fetch(`/api/v1/recipes/search?size=10&page=1&${productsQuery}`)
            .then(res => res.json())
    }
}