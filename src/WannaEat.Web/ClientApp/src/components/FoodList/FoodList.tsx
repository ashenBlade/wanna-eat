import React from 'react';
import './FoodList.tsx.css'
import {Food} from "../../entities/food";

export interface FoodListProps<TFood extends Food> {
    foods: TFood[],
    onChoose?: ((product: TFood) => void),
    emptyListPlaceholder?: string,
    onScrollToEnd?: () => (void)
}

const FoodList = <TFood extends Food>({foods, emptyListPlaceholder, onChoose}: FoodListProps<TFood>) => {
    const placeholder = emptyListPlaceholder ?? '';
    
    const onChooseInner = (f: TFood) => onChoose ? onChoose(f) : null;
    
    return (
        <div className={'h-100'}>
            <div className={'food-scroll bg-light p-2 rounded-1 h-100'}>
                <ul className={'list-group rounded-1 h-0'}>
                    {
                        foods.length > 0
                            ? foods.map(f => (
                                <li key={f.name} onClick={_ => onChooseInner(f)} value={f.name}
                                    className={'list-group-item p-1 p-md-2 cursor-pointer'}>
                                    {f.name}
                                </li>))
                            : <p className={'text-center text-black'}>{placeholder}</p>
                    }
                </ul>
            </div>
        </div>
    );
};

export default FoodList;