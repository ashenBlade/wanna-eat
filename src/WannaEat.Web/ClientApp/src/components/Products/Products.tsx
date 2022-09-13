import React, {FC, useEffect, useState} from 'react';
import {Ingredient} from "../../entities/ingredient";
import {Recipe} from "../../entities/recipe";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";
import {IFoodService} from "../../interfaces/iFoodService";
import {IIngredientsRepository} from "../../interfaces/iIngredientRepository";

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
    
    const searchDelaySeconds = 0.2;
    
    const defaultPageSize = 15
    
    const resetTimeout = () => {
        window.clearTimeout(searchTimeout)
    };

    const searchProductsByName = (name: string) => {
        if (name.length < 3) {
            if (name.length === 0)
                ingredientsRepository.getProductsAsync(1, defaultPageSize).then(loaded => {
                    setProducts([...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))]);
                    setCurrentProductsPage(1)
                })
            return;
        }
        ingredientsRepository.findWithName(name, 1, defaultPageSize).then(loaded => {
            setProducts([...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))])
            setCurrentProductsPage(1)
        });
    }


    const selectedProductOnChoose = (sp: Ingredient) => {
        moveToProducts(sp)
    }

    const productOnChoose = (p: Ingredient) => {
        moveToSelected(p)
    }

    const [calculateButtonEnabled, setCalculateButtonEnabled] = useState(true)

    const onCalculateButtonClick = () => {
        setCalculateButtonEnabled(false);
        foodService.findRelevantDishes(selectedProducts).then(d => {
            setDishesNotFoundMessage(d.length === 0
                ? 'Ничего не нашлось('
                : '');
            setRecipes(d);
            setCalculateButtonEnabled(true);
        })
    }

    const redirectToRecipe = (r: Recipe) => {
        console.log(r.originUrl);
        window.open(r.originUrl, '_blank');
    }

    useEffect(() => {
        setCalculateButtonEnabled(selectedProducts.length !== 0)
    }, [selectedProducts]);

    useEffect(() => {
        resetTimeout();

        const handle = window.setTimeout(() => {
            searchProductsByName(productSearchName)
        }, searchDelaySeconds * 1000);
        setSearchTimeout(handle);
    }, [productSearchName]);

    useEffect(() => {
        ingredientsRepository.getProductsAsync(1, defaultPageSize).then(products => {
            setProducts(products)
        })
    }, [])

    return (
        <div className={'h-100'}>
            <div className={'triple-column h-100'}>
                <div className={'d-flex align-items-end'}>
                    <div className={'p-1 w-100 d-flex justify-content-between align-items-center'}>
                        <input className={'form-control'} type={'text'}
                               placeholder={'Что искать?'}
                               onChange={e => setProductSearchName(e.currentTarget.value)}/>
                    </div>
                </div>
                <div/>
                <div>
                    <button className={'btn btn-success w-100'} onClick={onCalculateButtonClick} disabled={!calculateButtonEnabled}>Подсчитать</button>
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