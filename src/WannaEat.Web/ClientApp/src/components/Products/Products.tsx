import React, {FC, useEffect, useState} from 'react';
import {Product} from "../../entities/product";
import {Dish} from "../../entities/dish";
import {IProductRepository} from "../../services/products.repository";
import {IDishRepository} from "../../services/dish.repository";
import {IFoodService} from "../../services/food.service";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";
import CookingApplianceMenu from "../CookingApplianceMenu/CookingApplianceMenu";
import {Food} from "../../entities/food";
import {precacheAndRoute} from "workbox-precaching";

interface ProductsPageProps {
    productsRepository: IProductRepository
    dishesRepository: IDishRepository
    foodService: IFoodService
}

const Products: FC<ProductsPageProps> = ({productsRepository, dishesRepository, foodService}) => {
    const [products, setProducts] = useState<Product[]>([]);
    const [dishes, setDishes] = useState<Dish[]>([]);
    const [selectedProducts, setSelectedProducts] = useState<Product[]>([]);
    
    const [searchTimeout, setSearchTimeout] = useState<number | null>(null);
    const [productSearchName, setProductSearchName] = useState('');
    
    const searchDelaySeconds = 1;
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
        }, searchDelaySeconds * 1000)
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
    

    const selectedProductOnChoose = (sp: Product) => {
        setSelectedProducts(selectedProducts.filter(s => s.id !== sp.id))
        setProducts([...products, sp])
    }
    
    const productOnChoose = (p: Product) => {
        setProducts([...products.filter(op => op.id !== p.id)]);
        setSelectedProducts([...selectedProducts, p]);
    }
    
    return (
        <div className={'h-100'}>
            <div className={'double-column h-100'}>
                <div className={'d-flex align-items-end'}>
                    <div className={'p-1 w-100 d-flex justify-content-between align-items-center'}>
                        <input className={'form-control'} type={'text'}
                               placeholder={'Что искать?'}
                               onChange={e => setProductSearchName(e.currentTarget.value)}/>
                    </div>
                </div>
                <div>
                    <div className={'d-flex justify-content-center p-2'}>
                        <CookingApplianceMenu applianceChangeCallback={selected => {}} appliances={[{name: 'Vlad', id: 1}, {name: 'Kirill', id: 2}, {name: 'Ivan', id: 3}, {name: 'Jenya', id: 4}]}/>
                    </div>
                </div>
                <div/>
                <div className={'grounded p-1 pb-2'}>
                    <FoodList onChoose={productOnChoose} foods={products}/>
                </div>
                <div className={'grounded p-1 pb-2'}>
                    <FoodList onChoose={selectedProductOnChoose} foods={selectedProducts}/>
                </div>
                <div className={'grounded p-1 pb-2'}>
                    <FoodList foods={dishes}/>
                </div>
            </div>
        </div>
    );
}

export default Products;