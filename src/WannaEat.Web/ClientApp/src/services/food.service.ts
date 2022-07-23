import {Product} from "../entities/product";
import {CookingAppliance} from "../entities/cooking-appliance";
import {Dish} from "../entities/dish";

export interface IFoodService {
    findRelevantDishes(products: Product[], cookingAppliances: CookingAppliance[] | null): Promise<Dish[]>
}

export class FoodService implements IFoodService {
    
    
    findRelevantDishes(products: Product[], cookingAppliances: CookingAppliance[] | null): Promise<Dish[]> {
        return Promise.resolve([]);
    }
}