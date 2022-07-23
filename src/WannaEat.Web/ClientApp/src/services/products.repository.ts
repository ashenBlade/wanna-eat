import {Product} from "../entities/product";

export interface IProductRepository {
    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]>
    getProductById(id: number): Promise<Product | null>
}

export class ProductRepository implements IProductRepository {
  
    getProductById(id: number): Promise<Product | null> {
        return Promise.resolve(null);
    }

    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]> {
        return Promise.resolve([]);
    }
}