import React, {FC, useEffect, useState} from 'react';
import {Product} from "../../entities/product";
import {Dish} from "../../entities/dish";
import {IProductsRepository} from "../../services/productsRepository";
import {IDishesRepository} from "../../services/dishesRepository";
import {IFoodService} from "../../services/foodService";
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
    const [selectedProducts, setSelectedProducts] = useState<Product[]>([]);
    const moveToSelected = (product: Product) => {
        setProducts([...products.filter(p => p.id !== product.id)])
        setSelectedProducts([...selectedProducts, product])
    }
    
    const moveToProducts = (product: Product) => {
        setSelectedProducts([...selectedProducts.filter(sp => sp.id !== product.id)])
        setProducts([...products, product])
    }
    
    const [dishes, setDishes] = useState<Dish[]>([]);
    const [cookingAppliances, setCookingAppliances] = useState<CookingAppliance[]>([]);
    const [dishesNotFoundMessage, setDishesNotFoundMessage] = useState('Здесь появится, то что можно приготовить');
    
    const [searchTimeout, setSearchTimeout] = useState(0);
    const [productSearchName, setProductSearchName] = useState('');
    
    const searchDelaySeconds = 0.5;
    
    const defaultPageSize = 15
    
    useEffect(() => {
        productsRepository.getProductsAsync(1, defaultPageSize).then(products => {
            setProducts(products)
        })
        cookingApplianceRepository.getCookingAppliancesAsync(1, defaultPageSize).then(a => {
            setCookingAppliances(a)
        })
    }, [])

    const resetTimeout = () => {
        window.clearTimeout(searchTimeout)
    }

    useEffect(() => {
        resetTimeout();

        const handle = window.setTimeout(() => {
            searchProductsByName(productSearchName)
        }, searchDelaySeconds * 1000);
        setSearchTimeout(handle);
    }, [productSearchName]);
    
    const searchProductsByName = (name: string) => {
        if (name.length < 3) {
            if (name.length === 0)
                productsRepository.getProductsAsync(1, defaultPageSize).then(prods => {
                    setProducts([...prods.filter(p => !selectedProducts.some(sp => sp.id === p.id))]);
                })
            return;
        }
        
        productsRepository.findWithName(name, 30).then(prods => {
            setProducts([...prods.filter(p => !selectedProducts.some(sp => sp.id === p.id))])
        });
    }
    
    useEffect(() => {
        setCalculateButtonEnabled(selectedProducts.length !== 0)
    }, [selectedProducts])
    

    const selectedProductOnChoose = (sp: Product) => {
        moveToProducts(sp)
    }
    
    const productOnChoose = (p: Product) => {
        moveToSelected(p)
    }
    
    const [calculateButtonEnabled, setCalculateButtonEnabled] = useState(true)
    
    const onCalculateButtonClick = () => {
        setCalculateButtonEnabled(false)
        foodService.findRelevantDishes(selectedProducts, cookingAppliances).then(d => {
            setDishesNotFoundMessage(d.length === 0 
                ? 'Ничего не нашлось('
                : '');
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
                <div title={'Что можно выбрать'} className={'grounded p-1 pb-2'}>
                    <FoodList onChoose={productOnChoose} foods={products}/>
                </div>
                <div title={'Что у вас имеется'} className={'grounded p-1 pb-2'}>
                    <FoodList onChoose={selectedProductOnChoose} foods={selectedProducts} emptyListPlaceholder={'Выберите продукты из списка слева'}/>
                </div>
                <div title={'Что можно приготовить'} className={'grounded p-1 pb-2'}>
                    <FoodList foods={dishes} emptyListPlaceholder={dishesNotFoundMessage}/>
                </div>
            </div>
        </div>
    );
}

export default Products;