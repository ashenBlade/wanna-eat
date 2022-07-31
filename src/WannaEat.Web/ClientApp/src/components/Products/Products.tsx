import React, {FC, useEffect, useState} from 'react';
import {Product} from "../../entities/product";
import {Dish} from "../../entities/dish";
import {IProductsRepository} from "../../services/productsRepository";
import {IDishesRepository} from "../../services/dishesRepository";
import {IFoodService} from "../../services/food.service";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";
import CookingApplianceMenu from "../CookingApplianceMenu/CookingApplianceMenu";
import {Food} from "../../entities/food";
import {precacheAndRoute} from "workbox-precaching";
import {ICookingApplianceRepository} from "../../services/cookingApplianceRepository";
import { CookingAppliance } from '../../entities/cooking-appliance';
import {useToggle} from "../../hooks/useToggle";

interface ProductsPageProps {
    productsRepository: IProductsRepository
    dishesRepository: IDishesRepository
    foodService: IFoodService
    cookingApplianceRepository: ICookingApplianceRepository
}

const Products: FC<ProductsPageProps> = ({productsRepository, dishesRepository, foodService, cookingApplianceRepository}) => {
    const [products, setProducts] = useState<Product[]>([]);
    const [dishes, setDishes] = useState<Dish[]>([]);
    const [selectedProducts, setSelectedProducts] = useState<Product[]>([]);
    const [cookingAppliances, setCookingAppliances] = useState<CookingAppliance[]>([]);
    
    const [searchTimeout, setSearchTimeout] = useState<number>(0);
    const [productSearchName, setProductSearchName] = useState('');
    
    const searchDelaySeconds = 1;
    useEffect(() => {
        productsRepository.getProductsAsync(1, 15).then(p => {
            setProducts(p)
        })
        cookingApplianceRepository.getCookingAppliancesAsync(1, 10).then(a => {
            setCookingAppliances(a)
        })
    }, [])

    const resetTimeout = () => {
        window.clearTimeout(searchTimeout)
    }

    useEffect(() => {
        resetTimeout();

        const handle = window.setTimeout(() => {
            searchName(productSearchName)
        }, searchDelaySeconds * 1000)
        setSearchTimeout(handle);
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
        setSelectedProducts([...selectedProducts.filter(s => s.id !== sp.id)])
        setProducts([...products, sp])
    }
    
    const productOnChoose = (p: Product) => {
        setProducts([...products.filter(op => op.id !== p.id)]);
        setSelectedProducts([...selectedProducts, p]);
    }
    
    const [calculateButtonEnabled, setCalculateButtonEnabled] = useState(true)
    
    const onCalculateButtonClick = () => {
        setCalculateButtonEnabled(false)
        foodService.findRelevantDishes(selectedProducts, cookingAppliances).then(d => {
            setDishes(d);
            setCalculateButtonEnabled(true);
        })
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
                        <CookingApplianceMenu applianceChangeCallback={selected => {}} appliances={cookingAppliances}/>
                    </div>
                </div>
                <div>
                    <button className={'btn btn-success'} onClick={onCalculateButtonClick} disabled={!calculateButtonEnabled}>Подсчитать</button>
                </div>
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