import {Container} from "reactstrap";
import {useState} from "react";

const App = () => {
    const [number, setNumber] = useState(1)
    
    return (
        <Container onClick={() => setNumber(number + 1)}>
            {number}
        </Container>
    )   
}

export default App