import NavBar from "./components/NavBar/NavBar";
import Products from "./components/Products/Products";
import './custom.css';
import {StubIngredientsRepository} from "./services/stubIngredientsRepository";
import {StubFoodService} from "./services/stubFoodService";

const App = () => {
    const productsRepo = new StubIngredientsRepository()
    const foodService = new StubFoodService()
    return (
        <div className={'page-layout'}>
            <div>
                <NavBar/>
            </div>
            <div className={'container-lg page py-2'}>
                <Products ingredientsRepository={productsRepo} 
                          foodService={foodService}/>
            </div>
        </div>
    )   
}

export default App