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
    
    async getDishByIdAsync(id: number): Promise<Dish | null> {
        return await fetch(`/api/v1/dishes/${id}`).then(res => res.json());
    }

    async getDishesAsync(pageNumber: number, pageSize: number): Promise<Dish[]> {
        if (pageNumber < 1) {
            throw new Error('Page number could not be less than 1. Given: ' + pageNumber);
        }
        
        if (pageSize < 1) {
            throw new Error('Page size could not be less than 1. Given: ' + pageSize);
        }
        
        return await fetch(`/api/v1/dishes?page-number=${pageNumber}&page-size=${pageSize}`).then(res => res.json());
    }
}