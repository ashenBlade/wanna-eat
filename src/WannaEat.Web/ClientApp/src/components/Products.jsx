import React, {useEffect, useState} from 'react';

const Products = () => {
    
    const [products, setProducts] = useState([])
    
    const getProducts = () => {
        return fetch('api/v1/products?n=1&s=10')
            .then(res => res.json())
    }
    
    useEffect(async () => {
        const p = await getProducts()
        setProducts(p)
    }, [])
    
    return (
        <div>
            <h1>
                This is page with Products:
            </h1>
            <ul>
                {products.map(p => (
                    <li>
                        {p.id} {p.name}    
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default Products;