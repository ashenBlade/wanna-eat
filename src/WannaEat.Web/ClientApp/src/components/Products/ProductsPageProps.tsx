import {IIngredientsRepository} from "../../interfaces/iIngredientRepository";
import {IRecipeService} from "../../interfaces/IRecipeService";

export interface ProductsPageProps {
    ingredientsRepository: IIngredientsRepository
    foodService: IRecipeService
}