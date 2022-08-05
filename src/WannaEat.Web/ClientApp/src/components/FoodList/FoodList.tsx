import React, {useEffect, useRef, useState} from 'react';
import {Food} from "../../entities/food";
import './FoodList.tsx.css'

export interface FoodListProps<TFood extends Food> {
    foods: TFood[],
    onChoose?: ((product: TFood) => void) | undefined,
    emptyListPlaceholder?: string,
    onScrollToEnd?: () => (void)
}

const FoodList = <TFood extends Food>({foods, onChoose, emptyListPlaceholder, onScrollToEnd}: FoodListProps<TFood>) => {
    const searchById = (id: number) => foods.filter(f => f.id === id)[0] ?? null;
    const placeholder = emptyListPlaceholder ?? '';
    const foodOnClick = (e: React.SyntheticEvent<HTMLLIElement, MouseEvent>) => {
        e.stopPropagation()
        const food = searchById(Number(e.currentTarget.value));
        if (onChoose && food) {
            onChoose(food)
        }
    }
    
    const [observer, setObserver] = useState(new IntersectionObserver((entries, observer1) => {
        if (onScrollToEnd) {
            onScrollToEnd()
        }
    }))
    const lastElementRef = useRef(null)
    useEffect(() => {
        if (lastElementRef.current !== null) {
                observer.observe(lastElementRef.current)
            
        } else {
            setTimeout(() => {
                if (lastElementRef.current !== null)
                    observer.observe(lastElementRef.current)    
                else
                    console.log('Again null')
            }, 1000)
        }
    }, [lastElementRef])
    
    return (
        <div className={'h-100'}>
            <div className={'bg-light d-grid p-2 rounded-1 h-100'}>
                <div className={'scroll-list'}>
                    <ul className={'list-group rounded-1'} style={{
                        maxHeight: '100px'
                    }}>
                        { 
                            foods.length > 0 
                                ? 
                                foods.map(f => (
                                    <li key={f.id} value={f.id} onClick={foodOnClick}
                                        className={'list-group-item cursor-pointer'}>
                                        {f.name}
                                    </li>))
                                :
                                    <p className={'text-center text-black'}>{placeholder}</p>
                        }
                                    <li style={{backgroundColor: 'red'}} ref={lastElementRef}/>
                    </ul>
                </div>
            </div>
        </div>
    );
};

export default FoodList;