import {Ingredient} from "../entities/ingredient";

export interface IIngredientsRepository {
    getProductsAsync(pageNumber: number, pageSize: number): Promise<Ingredient[]>
    getProductById(id: number): Promise<Ingredient | null>
    findWithName(name: string, pageNumber: number, max: number): Promise<Ingredient[]>
}