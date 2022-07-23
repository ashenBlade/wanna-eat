import {Product} from "../entities/product";
import Products from "../components/Products";

export interface IProductRepository {
    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]>
    getProductById(id: number): Promise<Product | null>
    findWithName(name: string): Promise<Product[]>
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
        return Promise.resolve(ProductRepository.defaultProducts.find(p => p.id === id) ?? null);
    }

    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]> {
        const start = (pageNumber - 1) * pageSize;
        const end = start + pageSize
        return Promise.resolve([...ProductRepository.defaultProducts.slice(start, end)]);
    }

    findWithName(name: string): Promise<Product[]> {
        name = name.toLowerCase()
        return Promise.resolve(ProductRepository.defaultProducts.filter(p => p.name.toLowerCase().indexOf(name) !== -1));
    }
}