import {Product} from "../entities/product";

interface IProductRepository {
    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]>
    getProductById(id: number): Promise<Product | null>
}

class ProductRepository implements IProductRepository {
    constructor(readonly serverAddress: string) {
    }
    
    getProductById(id: number): Promise<Product | null> {
        return Promise.resolve(null);
    }

    getProductsAsync(pageNumber: number, pageSize: number): Promise<Product[]> {
        return Promise.resolve([]);
    }
}