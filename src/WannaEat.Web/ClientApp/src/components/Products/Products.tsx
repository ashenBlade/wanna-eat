import React, {FC, useEffect, useReducer, useState} from 'react';
import {Ingredient} from "../../entities/ingredient";
import {Recipe} from "../../entities/recipe";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";
import {ProductsPageProps} from "./ProductsPageProps";
import {usePagination} from "../../hooks/usePagination/usePagination";

const Products: FC<ProductsPageProps> = ({ingredientsRepository, foodService}) => {
    const [products, ] = useState<Ingredient[]>([]);
    const [selectedProducts, ] = useState<Ingredient[]>([]);
    const [productsLoading, setProductsLoading] = useState(false);
    const [canDownloadMoreProducts, setCanDownloadMoreProducts] = useState(true);
    const [, rerender] = useReducer(x => x + 1, 0);
    const [, toNextProductsPage, toPrevProductsPage, resetProductsPage] = usePagination({
        minPage: 1,
        startPage: 1,
    });


    const [recipes, setRecipes] = useState<Recipe[]>([]);
    const [recipesListMessage, setRecipesListMessage] = useState('Здесь появится, то что можно приготовить');
    const [recipesVisible, setRecipesVisible] = useState(false);
    const [recipesLoading, setRecipesLoading] = useState(false);

    const [searchTimeout, setSearchTimeout] = useState(0);
    const [productSearchName, setProductSearchName] = useState('');

    const searchDelaySeconds = 0.3;
    const defaultPageSize = 40;


    const isInLoadingState = () => productsLoading || recipesLoading;
    const shouldUseProductName = () => productSearchName.length > 3;
    const anyProductSelected = () => selectedProducts.length !== 0;
    const isProductSearchNameEmpty = () => productSearchName.length === 0;

    const moveToSelected = (product: Ingredient) => {
        products.splice(products.indexOf(product), 1);
        selectedProducts.push(product);
        rerender();
    }

    const moveToProducts = (product: Ingredient) => {
        selectedProducts.splice(selectedProducts.indexOf(product), 1);
        products.push(product);
        rerender();
    }

    const filterUnselectedProducts = (products: Ingredient[]) => [...products.filter(p => !selectedProducts.some(sp => sp.id === p.id))];

    const appendToProducts = (loaded: Ingredient[]) => {
        products.push(...loaded);
        rerender();
    };

    const loadFirstProductPage = async () => {
        if (isInLoadingState()) {
            return;
        }
        setProductsLoading(true);
        try {
            const loaded = await ingredientsRepository.getProductsAsync( 1, defaultPageSize);
            setCanDownloadMoreProducts(loaded.length >= defaultPageSize);
            products.length = 0;
            products.push(...(filterUnselectedProducts(loaded)))
            resetProductsPage();
            rerender();
        } catch (e) {
            console.error('Error while loading products by page', e);
            throw e;
        } finally {
            setProductsLoading(false);
        }
    }

    const loadProductsFirstPageBySearchName = async () => {
        if (isInLoadingState()) {
            return;
        }
        setProductsLoading(true);
        try {
            const loaded = await ingredientsRepository.findWithName(productSearchName, 1, defaultPageSize);
            const filtered = filterUnselectedProducts(loaded);
            setCanDownloadMoreProducts(filtered.length >= defaultPageSize);
            products.length = 0;
            products.push(...filtered);
            resetProductsPage();
            rerender();
        } catch (e) {
            console.error('Error while loading products by page', e);
        } finally {
            setProductsLoading(false);
            resetProductsPage();
        }
    }


    const loadProductsPaged = async (page: number) => {
        setProductsLoading(true);
        try {
            return await ingredientsRepository.getProductsAsync(page, defaultPageSize);
        } finally {
            setProductsLoading(false);
        }
    }
    const loadProductsByName = async (name: string, page: number) => {
        setProductsLoading(true);
        try {
            return await ingredientsRepository.findWithName(name, page, defaultPageSize);
        } finally {
            setProductsLoading(false);
        }
    }

    const onProductPageScrollToEnd = async () => {
        if (productsLoading || !canDownloadMoreProducts) {
            return;
        }
        await loadNextProductsPage();
    }


    const loadNextProductsPage = async () => {
        if (isInLoadingState() || !canDownloadMoreProducts) {
            return;
        }

        setProductsLoading(true);
        const nextPage = toNextProductsPage();
        try {
            const loaded = await (shouldUseProductName()
                ? loadProductsByName(productSearchName, nextPage)
                : loadProductsPaged(nextPage));
            setCanDownloadMoreProducts(loaded.length >= defaultPageSize);
            appendToProducts(loaded);
        } catch (e) {
            console.error('Could not download next product page', e);
            toPrevProductsPage();
            setCanDownloadMoreProducts(false);
        } finally {
            setProductsLoading(false);
        }
    }

    const selectedProductOnChoose = (sp: Ingredient) => {
        if (recipesLoading) {
            return;
        }
        moveToProducts(sp);
    }

    const productOnChoose = (p: Ingredient) => {
        if (recipesLoading) {
            return;
        }
        moveToSelected(p);
        setProductSearchName('');
    }

    const redirectToRecipe = (r: Recipe) => {
        window.open(r.originUrl, '_blank');
    }


    useEffect(() => {
        window.clearTimeout(searchTimeout);

        const handle = window.setTimeout(async () => {
            if (shouldUseProductName()) {
                await loadProductsFirstPageBySearchName();
            } else if (isProductSearchNameEmpty()) {
                await loadFirstProductPage();
            }
        }, searchDelaySeconds * 1000);
        setSearchTimeout(handle);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [productSearchName]);

    const onCalculateButtonClick = async () => {
        setRecipesLoading(true);
        try {
            const recipes = await foodService.findRelevantRecipes(selectedProducts);
            setRecipesListMessage(recipes.length === 0
                ? 'Ничего не нашлось('
                : '');
            showRecipes(recipes);
        } catch (e) {
            setRecipesListMessage('Во время запроса произошла ошибка. Приносим извинения')
        } finally {
            setRecipesLoading(false);
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
                               disabled={recipesLoading}
                               onChange={e => setProductSearchName(e.currentTarget.value)}/>
                    </div>
                </div>
                <div/>
                <div className={'p-1'}>
                    <button className={'btn btn-success w-100'}
                            onClick={onCalculateButtonClick}
                            disabled={!anyProductSelected() || recipesLoading}>
                        Подсчитать
                    </button>
                </div>
                <div title={'Что можно выбрать'} className={'grounded p-1 pb-2'}>
                    <div className={'text-center pb-md-2 pb-1'}>
                        <span>Нашли</span>
                    </div>
                    <FoodList onChoose={productOnChoose}
                              foods={products}
                              emptyListPlaceholder={'Здесь появятся найденные продукты'}
                              isLoading={productsLoading}
                              onScrollToEnd={onProductPageScrollToEnd}/>
                </div>
                <hr className={'d-block m-0 d-md-none'}/>
                <div title={'Что у вас имеется'} className={'grounded p-1 pb-2'}>
                    <div className="text-center pb-md-2 pb-1"><span>Выбрали</span></div>
                    <FoodList onChoose={selectedProductOnChoose}
                              foods={selectedProducts}
                              additionalAction={{
                                  hint: 'У меня этого нет',
                                  sign: '✕'
                              }}
                              emptyListPlaceholder={'Здесь будут выбранные продукты'}/>
                </div>

                <div className={`d-md-block recipes`}>
                    <div className={'d-block d-md-none'}>
                        <button className={'btn'}
                                onClick={onBackButtonClick}>
                            ❮ Назад
                        </button>
                    </div>
                    <div title={'Это список найденных рецептов'}
                         className={'grounded p-1 d-flex h-100 pb-2'}>
                        <div className="text-center pb-md-2 pb-1">
                            <span>Можно приготовить</span>
                        </div>
                        <FoodList foods={recipes}
                                  onChoose={redirectToRecipe}
                                  emptyListPlaceholder={recipesListMessage}
                                  isLoading={recipesLoading}/>
                    </div>
                </div>

            </div>
        </div>
    );
}

export default Products;