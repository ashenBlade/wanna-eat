import NavBar from "./components/NavBar";
import Products from "./components/Products/Products";
import {ProductsRepository} from "./services/productsRepository";
import {DishesRepository} from "./services/dishesRepository";
import {FoodService} from "./services/foodService";
import './custom.css';
import {CookingApplianceRepository} from "./services/cookingApplianceRepository";

const App = () => {
    const productsRepo = new ProductsRepository()
    const dishesRepo = new DishesRepository()
    const foodService = new FoodService()
    const cookingAppliancesRepo = new CookingApplianceRepository()
    return (
        <div className={'page-layout'}>
            <div>
                <NavBar/>
            </div>
            <div className={'container-lg page py-2'}>
                <Products cookingApplianceRepository={cookingAppliancesRepo}
                          productsRepository={productsRepo} 
                          dishesRepository={dishesRepo}
                          foodService={foodService}/>
            </div>
        </div>
    )   
}

export default App