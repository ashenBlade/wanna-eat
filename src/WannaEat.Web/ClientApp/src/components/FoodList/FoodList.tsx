import React, {Ref, useEffect} from 'react';
import './FoodList.tsx.css'
import {Food} from "../../entities/food";
import {FoodListProps} from "./FoodListProps";
import Loader from "../Loader/Loader";
import {useInView} from "react-intersection-observer";

const FoodList = <TFood extends Food>({foods, emptyListPlaceholder, onChoose, additionalAction, isLoading, onScrollToEnd}: FoodListProps<TFood>) => {
    const placeholder = emptyListPlaceholder ?? '';
    const {sign, hint} = additionalAction ?? {sign: undefined, hint: ''}
    const onChooseInner = (f: TFood) => onChoose ? onChoose(f) : null;



    const createFoodListElement = (f: TFood, ref?: Ref<any>) => sign === undefined
        ? (<li key={f.name}
               onClick={_ => onChooseInner(f)}
               value={f.name}
               ref={ref}
               className={'list-group-item p-1 p-md-2 cursor-pointer food-list-item'}>
            <span>
                {f.name}
            </span>
        </li>)
        : (<li key={f.name}
               value={f.name}
               ref={ref}
               className={'list-group-item p-1 p-md-2 food-list-item'}>
                <span>
                    {f.name}
                </span>
                <span onClick={() => onChooseInner(f)}
                      title={hint}
                      className={'cursor-pointer'}>
                    {sign}
                </span>
            </li>);

    const {ref, inView} = useInView({
        delay: 1,
        threshold: 1,
        triggerOnce: true
    });


    useEffect(() => {
        if (inView && onScrollToEnd) {
            onScrollToEnd();
        }
    }, [inView, onScrollToEnd]);


    const createFoodsList = (foods: TFood[]) => {
        if (foods.length === 0) {
            return (<></>)
        }

        const lastElementIndex = foods.length - 1;
        return foods.map((f, i) => createFoodListElement(f, i === lastElementIndex && onScrollToEnd ? ref : null))
    }

    return (
        <div className={'h-100'}>
            <div className={'food-scroll bg-light p-2 rounded-1 h-100'}>
                <ul className={'list-group rounded-1 h-0'}>
                    {
                        foods.length > 0
                            ? createFoodsList(foods)
                            : isLoading
                                ? <></>
                                :(<p className={'text-center text-black'}>
                                    {placeholder}
                                </p>)
                    }
                    {
                        isLoading
                            ? <div className={'justify-content-center d-flex'}>
                                <Loader color={'gray'}/>
                            </div>
                            : <></>
                    }
                </ul>
            </div>
        </div>
    );
};

export default FoodList;