import React, {FC, useEffect, useState} from 'react';
import {Ingredient} from "../../entities/ingredient";
import {Recipe} from "../../entities/recipe";
import './Products.tsx.css';
import FoodList from "../FoodList/FoodList";
import {ProductsPageProps} from "./ProductsPageProps";
import {usePagination} from "../../hooks/usePagination/usePagination";

const Products: FC<ProductsPageProps> = ({ingredientsRepository, foodService}) => {
    const [products, setProducts] = useState<Ingredient[]>([]);
    const [selectedProducts, setSelectedProducts] = useState<Ingredient[]>([]);
    const [productsLoading, setProductsLoading] = useState(false);
    const [canDownloadMoreProducts, setCanDownloadMoreProducts] = useState(true);
    const [productsPage, toNextProductsPage, toPrevProductsPage, resetProductsPage] = usePagination({
        minPage: 1,
        startPage: 1,
    });

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
    const [recipesLoading, setRecipesLoading] = useState(false);

    const [searchTimeout, setSearchTimeout] = useState(0);
    const [productSearchName, setProductSearchName] = useState('');

    const searchDelaySeconds = 0.3;

    const defaultPageSize = 15;

    const filterUnselectedProducts = (products: Ingredient[]) => [...products.filter(p => !selectedProducts.some(sp => sp.id === p.id))];

    const appendToProducts = (loaded: Ingredient[]) => {
        const resultProducts = filterUnselectedProducts([...products, ...loaded]);
        console.log('appendToProducts', resultProducts);
        setProducts(resultProducts);
    };

    const loadFirstProductPage = async () => {
        if (productsLoading || recipesLoading) {
            return;
        }
        setProductsLoading(true);
        try {
            const products = await ingredientsRepository.getProductsAsync( 1, defaultPageSize);
            const filtered = filterUnselectedProducts(products);
            setCanDownloadMoreProducts(products.length >= defaultPageSize);
            console.log(products.length >= defaultPageSize);
            setProducts(filtered);
            resetProductsPage();
        } catch (e) {
            console.error('Error while loading products by page', e);
            throw e;
        } finally {
            setProductsLoading(false);
        }
    }

    const loadProductsFirstPageBySearchName = async () => {
        if (productsLoading || recipesLoading) {
            return;
        }
        console.log('loadProductsFirstPageBySearchName');
        setProductsLoading(true);
        try {
            const products = await ingredientsRepository.findWithName( productSearchName, 1, defaultPageSize);
            const filtered = filterUnselectedProducts(products);
            setCanDownloadMoreProducts(filtered.length >= defaultPageSize);
            setProducts(filtered);
        } catch (e) {
            console.error('Error while loading products by page', e);
        } finally {
            setProductsLoading(false);
            resetProductsPage();
        }
    }

    const loadProductsPaged = async (page: number) => {
        console.log('loadExtraProductsPaged', page)
        setProductsLoading(true);
        try {
            return await ingredientsRepository.getProductsAsync(page, defaultPageSize);
        } finally {
            setProductsLoading(false);
        }
    }
    const loadProductsByName = async (name: string, page: number) => {
        console.log('loadExtraProductsByName', {page, name})
        setProductsLoading(true);
        try {
            return await ingredientsRepository.findWithName(name, page, defaultPageSize);
        } finally {
            setProductsLoading(false);
        }
    }

    const onProductPageScrollToEnd = async () => {
        console.log('onProductPageScrollToEnd')
        if (productsLoading || !canDownloadMoreProducts) {
            console.log('onProductPageScrollToEnd not loading products loading already')
            return;
        }
        await loadNextProductsPage();
    }

    const shouldUseProductName = () => productSearchName.length > 3;

    const loadNextProductsPage = async () => {
        if (productsLoading || !canDownloadMoreProducts) {
            console.log('loadNextProductsPage early return')
            return;
        }

        setProductsLoading(true);
        const nextPage = toNextProductsPage();
        console.log('loadNextProductsPage', nextPage);
        try {
            const loaded = await (shouldUseProductName()
                ?    loadProductsByName(productSearchName, nextPage)
                :    loadProductsPaged(nextPage));
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

    const [calculateButtonEnabled, setCalculateButtonEnabled] = useState(true);

    const redirectToRecipe = (r: Recipe) => {
        console.log(r.originUrl);
        window.open(r.originUrl, '_blank');
    }

    useEffect(() => {
        if (selectedProducts.length !== 0)
            setCalculateButtonEnabled(true);
    }, [selectedProducts]);


    const isProductSearchNameEmpty = () => productSearchName.length === 0;

    useEffect(() => {
        window.clearTimeout(searchTimeout);

        const handle = window.setTimeout(async () => {
            if (shouldUseProductName()) {
                console.log('useEffect with shouldUseProductName')
                await loadProductsFirstPageBySearchName();
            } else if (isProductSearchNameEmpty()) {
                console.log('useEffect with isProductSearchNameEmpty')
                await loadFirstProductPage();
            }
        }, searchDelaySeconds * 1000);
        setSearchTimeout(handle);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [productSearchName]);

    // useEffect(() => {
    //     loadFirstProductPage();
    //     console.log('fuck useeffect')
    // }, []);

    const onCalculateButtonClick = async () => {
        setCalculateButtonEnabled(false);
        setRecipesLoading(true);
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
                            disabled={!calculateButtonEnabled}>
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