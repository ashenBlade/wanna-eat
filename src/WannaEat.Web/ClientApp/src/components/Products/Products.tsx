import React, {FC, useEffect, useState} from 'react';
import {Product} from "../../entities/product";
import {Dish} from "../../entities/dish";
import {IProductsRepository} from "../../services/productsRepository";
import {IDishesRepository} from "../../services/dishesRepository";
import {IFoodService} from "../../services/foodService";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";
import CookingApplianceMenu from "../CookingApplianceMenu/CookingApplianceMenu";
import {ICookingApplianceRepository} from "../../services/cookingApplianceRepository";
import { CookingAppliance } from '../../entities/cooking-appliance';

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
    const [currentProductsPage, setCurrentProductsPage] = useState(1)
    
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
    const loadNextProductsPage = () => {
        if (productSearchName.length >= 3) {
            productsRepository.findWithName(productSearchName, currentProductsPage + 1, defaultPageSize).then(loaded => {
                console.log(loaded)
                setProducts([...products, ...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))])
                setCurrentProductsPage(currentProductsPage + 1)
                console.log('New page set')
            })
        } else {
            productsRepository.getProductsAsync(currentProductsPage + 1, defaultPageSize).then(loaded => {
                console.log(loaded)
                setProducts([...products, ...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))])
                setCurrentProductsPage(currentProductsPage + 1)
                console.log('New page set')
            })
        }
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
                productsRepository.getProductsAsync(1, defaultPageSize).then(loaded => {
                    console.log(loaded)
                    setProducts([...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))]);
                    setCurrentProductsPage(1)
                })
            return;
        }
        
        productsRepository.findWithName(name, 1, defaultPageSize).then(loaded => {
            setProducts([...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))])
            setCurrentProductsPage(1)
        });
    }
    
    useEffect(() => {
        console.log('Current page:', currentProductsPage)
    }, [currentProductsPage])
    
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
                    <FoodList onChoose={productOnChoose} onScrollToEnd={() => {
                        loadNextProductsPage()
                    }} foods={products}/>
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