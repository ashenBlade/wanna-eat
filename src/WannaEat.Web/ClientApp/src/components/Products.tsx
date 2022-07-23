import React, {useEffect, useState} from 'react';
import {Product} from "../entities/product";
import {Dish} from "../entities/dish";
import {IProductRepository} from "../services/products.repository";
import {IDishRepository} from "../services/dish.repository";
import {IFoodService} from "../services/food.service";

interface ProductsPageProps {
    productsRepository: IProductRepository
    dishesRepository: IDishRepository
    foodService: IFoodService
}

const Products: React.FC<ProductsPageProps> = ({productsRepository, dishesRepository, foodService}) => {
    const [products, setProducts] = useState<Product[]>([])
    const [dishes, setDishes] = useState<Dish[]>([])
    useEffect(() => {
        productsRepository.getProductsAsync(1, 10).then(p => setProducts(p))
        dishesRepository.getDishesAsync(1, 10).then(d => setDishes(d))
    }, [])
    return (
        <div className={'h-100'}>
            <div>
                <div>
                    This is products list:
                    <ul>
                        {products.map(p => (
                            <li>{p.name}</li>
                        ))}
                    </ul>
                </div>
                <div>
                    This is dishes list:
                    <ul>
                        {dishes.map(d => (
                            <li>
                                {d.name}
                            </li>
                        ))}
                    </ul>
                </div>
            </div>
        </div>
    );
};

export default Products;