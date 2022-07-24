import React, {FC} from 'react';
import {Food} from "../../entities/food";

export interface FoodListProps {
    foods: Food[],
    // selectable: boolean | undefined
}

const FoodList: FC<FoodListProps> = ({foods}) => {
    
    return (
        <ul className={'list-group'}>
            {foods.map(f => (
                <li className={'list-group-item'}>
                    {f.name}
                </li>
                )
            )}
        </ul>
    );
};

export default FoodList;