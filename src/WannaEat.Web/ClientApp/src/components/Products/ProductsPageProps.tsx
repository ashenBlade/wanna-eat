import {IIngredientsRepository} from "../../interfaces/iIngredientRepository";
import {IFoodService} from "../../interfaces/iFoodService";

export interface ProductsPageProps {
    ingredientsRepository: IIngredientsRepository
    foodService: IFoodService
}