import React from 'react';
import './FoodList.tsx.css'
import {Food} from "../../entities/food";
import {FoodListProps} from "./FoodListProps";
import Loader from "../Loader/Loader";

const FoodList = <TFood extends Food>({foods, emptyListPlaceholder, onChoose, additionalAction, isLoading}: FoodListProps<TFood>) => {
    const placeholder = emptyListPlaceholder ?? '';
    const {sign, hint} = additionalAction ?? {sign: undefined, hint: ''}
    const onChooseInner = (f: TFood) => onChoose ? onChoose(f) : null;

    const createFoodListElement = (f: TFood) => sign === undefined
        ? (<li key={f.name}
               onClick={_ => onChooseInner(f)}
               value={f.name}
               className={'list-group-item p-1 p-md-2 cursor-pointer food-list-item'}>
            <span>
                {f.name}
            </span>
        </li>)
        : (<li key={f.name}
               value={f.name}
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

    return (
        <div className={'h-100'}>
            <div className={'food-scroll bg-light p-2 rounded-1 h-100'}>
                <ul className={'list-group rounded-1 h-0'}>
                    {
                        isLoading
                            ? <div className={'justify-content-center d-flex'}><Loader color={'gray'}/></div>
                            : foods.length > 0
                                ? foods.map(f => createFoodListElement(f))
                                : (<p className={'text-center text-black'}>
                                    {placeholder}
                                </p>)
                    }
                </ul>
            </div>
        </div>
    );
};

export default FoodList;