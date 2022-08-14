import NavBar from "./components/NavBar/NavBar";
import Products from "./components/Products/Products";
import {IngredientsRepository} from "./services/ingredientsRepository";
import {FoodService} from "./services/foodService";
import './custom.css';

const App = () => {
    const productsRepo = new IngredientsRepository()
    const foodService = new FoodService()
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