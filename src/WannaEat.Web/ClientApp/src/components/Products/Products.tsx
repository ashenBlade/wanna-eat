import React, {FC, useEffect, useState} from 'react';
import {Ingredient} from "../../entities/ingredient";
import {Recipe} from "../../entities/recipe";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";
import {ProductsPageProps} from "./ProductsPageProps";

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

    const [recipes, setRecipes] = useState<Recipe[]>([]);
    const [recipesListMessage, setRecipesListMessage] = useState('Здесь появится, то что можно приготовить');
    const [recipesVisible, setRecipesVisible] = useState(false);

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
                })
            return;
        }
        ingredientsRepository.findWithName(name, 1, defaultPageSize).then(loaded => {
            setProducts([...loaded.filter(p => !selectedProducts.some(sp => sp.id === p.id))])
        });
    }


    const selectedProductOnChoose = (sp: Ingredient) => {
        moveToProducts(sp);
    }

    const productOnChoose = (p: Ingredient) => {
        moveToSelected(p);
        setProductSearchName('');
    }

    const [calculateButtonEnabled, setCalculateButtonEnabled] = useState(true)

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
            searchProductsByName(productSearchName);
        }, searchDelaySeconds * 1000);
        setSearchTimeout(handle);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [productSearchName]);

    useEffect(() => {
        ingredientsRepository.getProductsAsync(1, defaultPageSize).then(products => {
            setProducts(products)
        })
    }, [ingredientsRepository])

    const onCalculateButtonClick = async () => {
        setCalculateButtonEnabled(false);
        try {
            const recipes = await foodService.findRelevantRecipes(selectedProducts);
            setRecipesListMessage(recipes.length === 0
                ? 'Ничего не нашлось('
                : '');
            showRecipes(recipes);
        } catch (e) {
            setRecipesListMessage('Во время запроса произошла ошибка')
        } finally {
            setCalculateButtonEnabled(true);
        }
    }

    const showRecipes = (recipes: Recipe[]) => {
        setRecipes(recipes);
        setRecipesVisible(true);
    }

    const onBackButtonClick = () => {
        setRecipesVisible(false);
        setProductSearchName('');
    }

    return (
        <div className={'h-100'}>
            <div className={`triple-column h-100 ${recipesVisible ? 'show-results' : ''}`}>
                <div className={'align-items-end p-1'}>
                    <div className={'p-1 w-100 d-flex justify-content-between align-items-center'}>
                        <input className={'form-control'}
                               type={'text'}
                               placeholder={'Что искать?'}
                               value={productSearchName}
                               onChange={e => setProductSearchName(e.currentTarget.value)}/>
                    </div>
                </div>
                <div/>
                <div className={'p-1'}>
                    <button className={'btn btn-success w-100'} onClick={onCalculateButtonClick}
                            disabled={!calculateButtonEnabled}>Подсчитать
                    </button>
                </div>
                <div title={'Что можно выбрать'} className={'grounded p-1 pb-2'}>
                    <FoodList onChoose={productOnChoose} foods={products}/>
                </div>
                <hr className={'d-block m-0 d-md-none'}/>
                <div title={'Что у вас имеется'} className={'grounded p-1 pb-2'}>
                    <FoodList onChoose={selectedProductOnChoose}
                              foods={selectedProducts}
                              listElementActionSign={'✕'}
                              listElementSignHint={'У меня этого нет'}
                              emptyListPlaceholder={'Здесь будут выбранные продукты'}/>
                </div>

                <div className={`d-md-block recipes`}>
                    <div className={'d-block d-md-none'}>
                        <button className={'btn'}
                                onClick={onBackButtonClick}>
                            ❮ Назад
                        </button>
                    </div>
                    <div title={'Что можно приготовить'} className={'grounded p-1 d-flex h-100 pb-2'}>
                        <FoodList foods={recipes}
                                  onChoose={redirectToRecipe}
                                  emptyListPlaceholder={recipesListMessage}/>
                    </div>
                </div>

            </div>
        </div>
    );
}

export default Products;