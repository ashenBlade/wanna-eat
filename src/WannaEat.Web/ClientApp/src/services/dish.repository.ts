import { Dish } from "../entities/dish";

export interface IDishRepository {
    getDishesAsync(pageNumber: number, pageSize: number): Promise<Dish[]>
    getDishByIdAsync(id: number): Promise<Dish | null>
}

export class DishRepository implements IDishRepository{
    static defaultDishes: Dish[] = [
        {id: 1, name: 'Backed bread'},
        {id: 2, name: 'Fried chicken'},
        {id: 3, name: 'Rice with meat'},
        {id: 4, name: 'Fruit salad'},
        {id: 5, name: 'Cottage cheesecake'}
    ]
    
    getDishByIdAsync(id: number): Promise<Dish | null> {
        return Promise.resolve(DishRepository.defaultDishes.find(d => d.id === id) ?? null);
    }

    getDishesAsync(pageNumber: number, pageSize: number): Promise<Dish[]> {
        return Promise.resolve([...DishRepository.defaultDishes]);
    }
}