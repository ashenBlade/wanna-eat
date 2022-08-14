import React, {FC, useEffect, useState} from 'react';
import {Ingredient} from "../../entities/ingredient";
import {Recipe} from "../../entities/recipe";
import {IIngredientsRepository} from "../../services/ingredientsRepository";
import {IFoodService} from "../../services/foodService";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";

interface ProductsPageProps {
    ingredientsRepository: IIngredientsRepository
    foodService: IFoodService
}

const Products: FC<ProductsPageProps> = ({ingredientsRepository, foodService}) => {
    const [products, setProducts] = useState<Ingredient[]>([]);
    const [selectedProducts, setSelectedProducts] = useState<Ingredient[]>([]);
    const moveToSelected = (product: Ingredient) => {
        setProducts([...products.filter(p => p.id !== product.id)])
        setSelectedProducts([...selectedProducts, product])
    }
    
    const moveToProducts = (product: Ingredient) => {
        setSelectedProducts([...selectedProducts.filter(sp => sp.id !== product.id)])
        setProducts([...products, product])
    }
    const [currentProductsPage, setCurrentProductsPage] = useState(1)
    
    const [recipes, setRecipes] = useState<Recipe[]>([]);
    const [dishesNotFoundMessage, setDishesNotFoundMessage] = useState('Здесь появится, то что можно приготовить');
    
    const [searchTimeout, setSearchTimeout] = useState(0);
    const [productSearchName, setProductSearchName] = useState('');
    
    const searchDelaySeconds = 0.5;
    
    const defaultPageSize = 15
    
    useEffect(() => {
        ingredientsRepository.getProductsAsync(1, defaultPageSize).then(products => {
            setProducts(products)
        })
    }, [])

    const resetTimeout = () => {
        window.clearTimeout(searchTimeout)
    }
    const loadNextProductsPage = () => {
        if (productSearchName.length >= 3) {
            ingredientsRepository.findWithName(productSearchName, currentProductsPage + 1, defaultPageSize).then(loaded => {
                console.log(loaded)
                setProducts([...products, ...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))])
                setCurrentProductsPage(currentProductsPage + 1)
                console.log('New page set')
            })
        } else {
            ingredientsRepository.getProductsAsync(currentProductsPage + 1, defaultPageSize).then(loaded => {
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
                ingredientsRepository.getProductsAsync(1, defaultPageSize).then(loaded => {
                    console.log(loaded)
                    setProducts([...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))]);
                    setCurrentProductsPage(1)
                })
            return;
        }
        console.log('asdf')
        ingredientsRepository.findWithName(name, 1, defaultPageSize).then(loaded => {
            console.log(loaded)
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
    

    const selectedProductOnChoose = (sp: Ingredient) => {
        moveToProducts(sp)
    }
    
    const productOnChoose = (p: Ingredient) => {
        moveToSelected(p)
    }
    
    const [calculateButtonEnabled, setCalculateButtonEnabled] = useState(true)
    
    const onCalculateButtonClick = () => {
        setCalculateButtonEnabled(false)
        foodService.findRelevantDishes(selectedProducts).then(d => {
            setDishesNotFoundMessage(d.length === 0 
                ? 'Ничего не нашлось('
                : '');
            setRecipes(d);
            setCalculateButtonEnabled(true);
        })
    }
    
    const redirectToRecipe = (r: Recipe) => {
        console.log(r.link)
        window.open(r.link, '_blank');
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
                <div/>
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
                    <FoodList foods={recipes} onChoose={redirectToRecipe} emptyListPlaceholder={dishesNotFoundMessage}/>
                </div>
            </div>
        </div>
    );
}

export default Products;