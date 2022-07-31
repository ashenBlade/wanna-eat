import {Product} from "../entities/product";
import {CookingAppliance} from "../entities/cooking-appliance";
import {Dish} from "../entities/dish";
import {DishesRepository} from "./dishesRepository";

export interface IFoodService {
    findRelevantDishes(products: Product[], cookingAppliances?: CookingAppliance[]): Promise<Dish[]>
}

export class FoodService implements IFoodService {
    
    async findRelevantDishes(products: Product[], cookingAppliances?: CookingAppliance[]): Promise<Dish[]> {
        const productsQuery = products.map(p => `may-contain=${p.id}`).join('&')
        const appliancesQuery = cookingAppliances === undefined
            ? ''
            : '&' + (cookingAppliances.map(ca => `cook-with=${ca}`).join('&'))
        return await fetch(`/api/v1/dishes/relevant?page-size=10&page-number=1&${productsQuery}${appliancesQuery}`)
            .then(res => res.json())
    }
}