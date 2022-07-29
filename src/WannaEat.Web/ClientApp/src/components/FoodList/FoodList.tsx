import React, {FC} from 'react';
import {Food} from "../../entities/food";
import './FoodList.tsx.css'

export interface FoodListProps {
    foods: Food[],
    // selectable: boolean | undefined
}

const FoodList: FC<FoodListProps> = ({foods}) => {
    
    return (
        <div className={'h-100 p-1'}>
            <div className={'bg-light p-2 rounded-1 h-100 scroll-list'}>
                <ul className={'list-group rounded-1'}>
                    {foods.map(f => (
                            <li key={f.id} className={'list-group-item'}>
                                {f.name}
                            </li>
                        )
                    )}
                </ul>
            </div>
        </div>
    );
};

export default FoodList;