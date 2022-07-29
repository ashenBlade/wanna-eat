import React, {useEffect, useState} from 'react';
import {Product} from "../../entities/product";
import {Dish} from "../../entities/dish";
import {IProductRepository} from "../../services/products.repository";
import {IDishRepository} from "../../services/dish.repository";
import {IFoodService} from "../../services/food.service";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";

interface ProductsPageProps {
    productsRepository: IProductRepository
    dishesRepository: IDishRepository
    foodService: IFoodService
}

const Products: React.FC<ProductsPageProps> = ({productsRepository, dishesRepository, foodService}) => {
    const [products, setProducts] = useState<Product[]>([])
    const [dishes, setDishes] = useState<Dish[]>([])
    useEffect(() => {
        productsRepository.getProductsAsync(1, 10).then(p => {
            console.log(p)
            setProducts(p)
        })
        dishesRepository.getDishesAsync(1, 10).then(d => {
            console.log(d)
            setDishes(d)
        })
    }, [])
    
    const searchOnChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const name = e.target.value
        if (!name) {
            productsRepository.getProductsAsync(1, 10).then(p => setProducts(p))
            return
        }
        
        if (name.length < 3)
            return;
        productsRepository.findWithName(name, 10).then(p => {
            console.log(p)
            setProducts(p)
        })
    } 
    
    
    return (
        <div className={'h-100'}>
            <div className={'double-column h-100'}>
                <div className={'d-flex align-items-end'}>
                    <div className={'p-1 w-100 d-flex justify-content-between align-items-center'}>
                        <input className={'form-control'} type={'search'}
                               placeholder={'Что искать?'}
                               onChange={searchOnChange}/>
                        <div className={'ms-2'}>
                            <i className={'fa fa-solid fa-gear fa-xl rotate-90-hover'}></i>
                        </div>
                    </div>
                </div>
                <div/>
                <div className={'grounded p-1 pb-2'}>
                    <FoodList foods={products}/>
                </div>
                <div className={'grounded p-1 pb-2'}>
                    <FoodList foods={dishes}/>
                </div>
            </div>
        </div>
    );
};

export default Products;