import {Product} from "../entities/product";
import {CookingAppliance} from "../entities/cooking-appliance";
import {Dish} from "../entities/dish";
import {DishRepository} from "./dish.repository";

export interface IFoodService {
    findRelevantDishes(products: Product[], cookingAppliances: CookingAppliance[] | null): Promise<Dish[]>
}

export class FoodService implements IFoodService {
    
    async findRelevantDishes(products: Product[], cookingAppliances: CookingAppliance[] | null): Promise<Dish[]> {
        const x = new DishRepository();
        const dishes = await x.getDishesAsync(1, 10)
        const productNames = [...products.map(p => p.name.toLowerCase())]
        return [...dishes.filter(d => productNames.some(n => d.name.toLowerCase().indexOf(n) != -1))];
    }
}