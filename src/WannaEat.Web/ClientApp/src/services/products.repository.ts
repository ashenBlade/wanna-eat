import {Product} from "../entities/product";

export interface IProductRepository {
    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]>
    getProductById(id: number): Promise<Product | null>
    findWithName(name: string, max: number | undefined): Promise<Product[]>
}

export class ProductRepository implements IProductRepository {
    static defaultProducts: Product[] = [
        {id: 1, name: 'Banana'},
        {id: 2, name: 'Apple'},
        {id: 3, name: 'Rice'},
        {id: 4, name: 'Bread'},
        {id: 5, name: 'Milk'},
        {id: 6, name: 'Chicken'}
    ]
    
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
        return fetch(`/api/v1/products?page-size=${pageSize}&page-number=${pageNumber}`).then(res => res.json());
    }
    
    findWithName(name: string, max: number | undefined = 10): Promise<Product[]> {
        if (name.length < 3) {
            throw new Error('Product length could not be less than 3. Given: ' + name);
        }
        
        if (max === undefined) 
            max = 10;
        
        if (max < 1 || 100 < max) {
            throw new Error('Max return range must be in range 1 to 100. Given: ' + max);
        }
        return fetch(`/api/v1/products/search?name=${encodeURIComponent(name)}&max=${max}`).then(res => res.json())
    }
}