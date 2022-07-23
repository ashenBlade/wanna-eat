import {useState} from "react";
import NavBar from "./components/NavBar";
import Products from "./components/Products";
import {ProductRepository} from "./services/products.repository";
import {DishRepository} from "./services/dish.repository";
import {FoodService} from "./services/food.service";

const App = () => {
    const productsRepo = new ProductRepository()
    const dishesRepo = new DishRepository()
    const foodService = new FoodService()
    return (
        <div>
            <NavBar/>
            <Products productsRepository={productsRepo} dishesRepository={dishesRepo} foodService={foodService}/>
        </div>
    )   
}

export default App