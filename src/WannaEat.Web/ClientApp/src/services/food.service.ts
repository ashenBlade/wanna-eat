import {Product} from "../entities/product";

interface IFoodService {
    findRelevantDishes(products: Product[], cookingAppliances: CookingAppliance[] | null): Promise<Dish[]>
}

class FoodService implements IFoodService {
    
    
    findRelevantDishes(products: Product[], cookingAppliances: CookingAppliance[] | null): Promise<Dish[]> {
        return Promise.resolve([]);
    }
}