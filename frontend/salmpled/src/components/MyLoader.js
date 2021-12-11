
import { Spinner, Container } from "react-bootstrap"
const MyLoader = () => {
    return (
        <Container className="d-flex mt-4 justify-content-center">
            <Spinner animation="border" role="status">
                <span className="visually-hidden">Loading...</span>
            </Spinner>
        </Container>
    )
}

export default MyLoader