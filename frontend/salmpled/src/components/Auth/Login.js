import { AuthContext } from "../../providers/AuthContext";
import { useContext } from "react";
import { signInWithGoogle } from "../../services/firebase";
import { Redirect } from "react-router";
import { useHistory } from "react-router-dom";
import { Container } from "react-bootstrap";
import { myHistory } from "../..";

export default function Login(props) {
    const { user, loading } = useContext(AuthContext);
    const path = !props.location.state?.from ? "/dashboard" : props.location.state.from.pathname;

    return (
        <Container>
          {!!user ? (
            myHistory.push(path)
          ) : (
            <div>
              <p>Please Sign In</p>
              <button onClick={signInWithGoogle} > Sign In With Google </button>
            </div>
          )}
        </Container>
      );
}