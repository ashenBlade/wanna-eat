import {Dish} from "../entities/dish";
import {Ingredient} from "../entities/ingredient";

export interface IFoodService {
    findRelevantDishes(products: Ingredient[]): Promise<Dish[]>
}

export class FoodService implements IFoodService {
    async findRelevantDishes(products: Ingredient[]): Promise<Dish[]> {
        const productsQuery = products.map(p => `include=${p.id}`).join('&')
        return await fetch(`/api/v1/dishes/relevant?size=10&page=1&${productsQuery}`, {
            method: 'POST'
        })
            .then(res => res.json())
    }
}