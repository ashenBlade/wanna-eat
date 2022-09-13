import NavBar from "./components/NavBar/NavBar";
import Products from "./components/Products/Products";
import './custom.css';
import {StubIngredientsRepository} from "./services/stubIngredientsRepository";
import {StubFoodService} from "./services/stubFoodService";
import {ExternalIngredientsRepository} from "./services/externalIngredientsRepository";
import {IIngredientsRepository} from "./interfaces/iIngredientRepository";
import {IFoodService} from "./interfaces/iFoodService";
import {ExternalFoodService} from "./services/externalFoodService";

const isProduction = process.env.NODE_ENV?.toLowerCase().indexOf('production') !== -1 ?? false; 
const useStubs = process.env.REACT_APP_USE_STUBS?.toLowerCase() === "true" ?? false;

const shouldUseStubs = () => !isProduction && useStubs;

const getIngredientsRepository = (): IIngredientsRepository => {
    return shouldUseStubs() 
        ? new StubIngredientsRepository()
        : new ExternalIngredientsRepository();
}

const getFoodService = (): IFoodService => {
    return shouldUseStubs()
        ? new StubFoodService()
        : new ExternalFoodService();
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