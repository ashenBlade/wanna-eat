import React from 'react';
import {Food} from "../../entities/food";
import './FoodList.tsx.css'

export interface FoodListProps<TFood extends Food> {
    foods: TFood[],
    onChoose?: ((product: TFood) => void) | undefined,
    emptyListPlaceholder?: string
}

const FoodList = <TFood extends Food>({foods, onChoose, emptyListPlaceholder}: FoodListProps<TFood>) => {
    const searchById = (id: number) => foods.filter(f => f.id === id)[0] ?? null;
    const placeholder = emptyListPlaceholder ?? '';
    const foodOnClick = (e: React.SyntheticEvent<HTMLLIElement, MouseEvent>) => {
        e.stopPropagation()
        const food = searchById(Number(e.currentTarget.value));
        if (onChoose && food) {
            onChoose(food)
        }
    }
    
    
    return (
        <div className={'h-100'}>
            <div className={'bg-light d-grid p-2 rounded-1 h-100'}>
                <div className={'scroll-list'}>
                    <ul className={'list-group rounded-1'} style={{
                        maxHeight: '100px'
                    }}>
                        { 
                            foods.length > 0 ? foods.map(f => (
                                <li key={f.id} value={f.id} onClick={foodOnClick} className={'list-group-item cursor-pointer'}>
                                    {f.name}
                                </li>
                            )
                        ) : <p className={'text-center text-black'}>{placeholder}</p>}
                    </ul>
                </div>
            </div>
        </div>
    );
};

export default FoodList;