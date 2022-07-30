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
    const [searchTimeout, setSearchTimeout] = useState<number | null>(null)
    const [productSearchName, setProductSearchName] = useState('');
    const timeoutDelaySeconds = 1;
    useEffect(() => {
        productsRepository.getProductsAsync(1, 15).then(p => {
            setProducts(p)
        })
    }, [])
    
    const resetTimeout = () => {
        if (searchTimeout !== null) {
            window.clearTimeout(searchTimeout)
            setSearchTimeout(null)
        }
    }
    
    useEffect(() => {
        resetTimeout();
        
        const callback = window.setTimeout(() => {
            searchName(productSearchName)
        }, timeoutDelaySeconds * 1000)
        setSearchTimeout(callback);
    }, [productSearchName]);
    
    
    const searchName = (name: string) => {
        if (name.length === 0) {
            productsRepository.getProductsAsync(1, 35).then(p => {
                setProducts(p);
            })
            return;
        }
        if (name?.length < 3)
            return;
        
        productsRepository.findWithName(name, 30).then(p => {
            setProducts(p)
        });
    }
    
    
    return (
        <div className={'h-100'}>
            <div className={'double-column h-100'}>
                <div className={'d-flex align-items-end'}>
                    <div className={'p-1 w-100 d-flex justify-content-between align-items-center'}>
                        <input className={'form-control'} type={'text'}
                               placeholder={'Что искать?'}
                               onChange={e => setProductSearchName(e.currentTarget.value)}/>
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