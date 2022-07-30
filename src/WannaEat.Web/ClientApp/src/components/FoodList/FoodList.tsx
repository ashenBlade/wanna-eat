import React, {FC} from 'react';
import {Food} from "../../entities/food";
import './FoodList.tsx.css'

export interface FoodListProps {
    foods: Food[],
}

const FoodList: FC<FoodListProps> = ({foods}) => {
    
    return (
        <div className={'h-100'}>
            <div className={'bg-light d-grid p-2 rounded-1 h-100'}>
                <div className={'scroll-list'}>
                    <ul className={'list-group rounded-1'} style={{
                        maxHeight: '100px'
                    }}>
                        {foods.map(f => (
                                <li key={f.id} className={'list-group-item'}>
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