import React from 'react';
import {Food} from "../../entities/food";
import './FoodList.tsx.css'

export interface FoodListProps<TFood extends Food> {
    foods: TFood[],
    onChoose?: ((product: TFood) => void) | undefined,
}

const FoodList = <TFood extends Food>({foods, onChoose}: FoodListProps<TFood>) => {
    const searchById = (id: number) => foods.filter(f => f.id === id)[0] ?? null;
    const foodOnClick = (e: React.SyntheticEvent<HTMLLIElement, MouseEvent>) => {
        e.stopPropagation()
        const food = searchById(Number(e.currentTarget.value));
        console.log(food)
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
                        {foods.map(f => (
                                <li key={f.id} value={f.id} onClick={foodOnClick} className={'list-group-item'}>
                                    {f.name}
                                </li>
                            )
                        )}
                    </ul>
                </div>
            </div>
        </div>
    );
};

export default FoodList;