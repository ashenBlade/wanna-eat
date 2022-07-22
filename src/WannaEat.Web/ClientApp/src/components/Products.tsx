import React, {useState} from 'react';
import {Container} from "reactstrap";

const Products = () => {
    const [products, setProducts] = useState([])
    return (
        <div>
            <Container>
                This is products page
            </Container>
        </div>
    );
};

export default Products;