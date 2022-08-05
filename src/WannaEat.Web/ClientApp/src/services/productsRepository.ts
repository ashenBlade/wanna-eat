import {Product} from "../entities/product";

export interface IProductsRepository {
    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]>
    getProductById(id: number): Promise<Product | null>
    findWithName(name: string, pageNumber: number, max: number): Promise<Product[]>
}

export class ProductsRepository implements IProductsRepository {
    getProductById(id: number): Promise<Product | null> {
        return fetch(`api/v1/products/${id}`).then(res => res.json());
    }

    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]> {
        if (pageNumber < 1) {
            throw new Error('Page number could not be less than 1. Given: ' + pageNumber);
        }
        
        if (pageSize < 1) {
            throw new Error('Page size could not be less than 1. Given: ' + pageSize);
        }
        return fetch(`/api/v1/products?s=${pageSize}&n=${pageNumber}`).then(res => res.json());
    }
    
    findWithName(name: string, pageNumber: number, max: number = 10): Promise<Product[]> {
        if (name.length < 3) {
            throw new Error('Product length could not be less than 3. Given: ' + name);
        }
        
        if (max === undefined) 
            max = 10;
        
        if (max < 1 || 100 < max) {
            throw new Error('Max return range must be in range 1 to 100. Given: ' + max);
        }
        
        if (pageNumber < 1) {
            throw new Error(`Page number can not be less than 1. Given: ${pageNumber}`)
        }
        return fetch(`/api/v1/products/search?name=${encodeURIComponent(name)}&page=${pageNumber}&size=${max}`).then(res => res.json())
    }
}