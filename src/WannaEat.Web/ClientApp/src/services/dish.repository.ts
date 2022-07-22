interface IDishRepository {
    getDishesAsync(pageNumber: number, pageSize: number): Promise<Dish[]>
    getDishByIdAsync(id: number): Promise<Dish | null>
}

class DishRepository implements IDishRepository{
    getDishByIdAsync(id: number): Promise<Dish | null> {
        return Promise.resolve(null);
    }

    getDishesAsync(pageNumber: number, pageSize: number): Promise<Dish[]> {
        return Promise.resolve([]);
    }
}