import NavBar from "./components/NavBar/NavBar";
import Products from "./components/Products/Products";
import './custom.css';
import {StubIngredientsRepository} from "./services/stubIngredientsRepository";
import {StubFoodService} from "./services/stubFoodService";
import {ExternalIngredientsRepository} from "./services/externalIngredientsRepository";
import {IIngredientsRepository} from "./interfaces/iIngredientRepository";
import {IRecipeService} from "./interfaces/IRecipeService";
import {ExternalRecipeService} from "./services/externalRecipeService";

const isProduction = process.env.NODE_ENV?.toLowerCase().indexOf('production') !== -1 ?? false; 
const useStubs = process.env.REACT_APP_USE_STUBS?.toLowerCase() === "true" ?? false;

const shouldUseStubs = () => !isProduction && useStubs;

const getIngredientsRepository = (): IIngredientsRepository => {
    return shouldUseStubs() 
        ? new StubIngredientsRepository()
        : new ExternalIngredientsRepository();
}

const getFoodService = (): IRecipeService => {
    return shouldUseStubs()
        ? new StubFoodService()
        : new ExternalRecipeService();
}


const App = () => {
    const ingredientsRepository = getIngredientsRepository();
    const foodService = getFoodService();
    return (
        <div className={'page-layout'}>
            <div>
                <NavBar/>
            </div>
            <div className={'container-lg page py-2'}>
                <Products ingredientsRepository={ingredientsRepository} 
                          foodService={foodService}/>
            </div>
        </div>
    )   
}

export default App