import React, { useEffect, useContext, useState } from "react";
import { BrowserRouter as Router, Switch, Route, Link, useLocation, Redirect } from "react-router-dom";
import { Container, Spinner } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import "./App.css";
import { axinstance } from "./services/axios";
import { withRouter } from "react-router";
import { ToastContainer } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

//MY PROVIDERS
import { AuthProvider, AuthContext } from "./providers/AuthContext";
//My COMPONENTS
import Dashboard from "./components/Dashboard";
import Login from "./components/Auth/Login";
import SetUsername from "./components/Auth/SetUsername";
import Header from './components/Header';
import MyLoader from "./components/MyLoader";
import EditSamplePack from "./components/EditSamplePack";
import Home from './components/Home';
import ViewSamplePack from './components/ViewSamplePack';
import ViewPlaylist from "./components/ViewPlaylist";
import UserProfile from "./components/UserProfile";
import EditPlaylist from './components/EditPlaylist';

function PrivateRoute({ children, ...rest }) {
  const { user, loading } = useContext(AuthContext);
  
  const [username, setUsername] = useState(null);
  
  const [usernameLoading, setUsernameLoading] = useState(true)

  useEffect(() => {
    console.log("Loading", user)
    if (!!user && !loading) {
      setUsernameLoading(true)
      axinstance.get('user/dashboard').then((res) => {
        if(res.data.data) {
          const username = res.data.data.username
          setUsername((previousState) => username);
          setUsernameLoading(false)
        }
      })
    }
  }, [user, loading, username])
  
  return (
    <>
      {
        loading ? <MyLoader /> : (
          <Route {...rest} render={({ location }) => {
            return !!user
              ?
              usernameLoading ?
              <MyLoader/> : 
              !!username ?
                children
                :
                <SetUsername username={username}/>
              : (<Redirect to={{
                pathname: '/login',
                state: { from: location }
              }} />)
          }} />)
      }
    </>
  )
}


function NoMatch() {
  let location = useLocation();
  return (
    <div>
      <h3>
        <code>{location.pathname}</code>  Not Found
      </h3>
    </div>
  );
}

function ErrorPage() {
  let location = useLocation();
  const str = "An Error Ocurred ;<("
  return (
    <div>
      <h3>
        <code>  {str} </code>
      </h3>
    </div>
  );
}



function App() {
  return (
        <div className="App">
          <Header />
          <Container>
            <Switch>
              <Route path="/" exact component={Home} />
              <Route path="/login" exact component={Login} />
              <Route path="/samplepack/view/:id"  component={ViewSamplePack} />
              <Route path="/sampleplaylist/view/:id"  component={ViewPlaylist} />
              <Route path="/user/view/:username" component={UserProfile} />
              <Route path="/err" exact component={ErrorPage} />
              <PrivateRoute path="/dashboard">
                <Dashboard />
              </PrivateRoute>
              <PrivateRoute path="/samplepack/edit/:id">
                <EditSamplePack />
              </PrivateRoute>
              <PrivateRoute path="/sampleplaylist/edit/:id">
                <EditPlaylist/>
              </PrivateRoute>
              <Route component={NoMatch} />
            </Switch>
            <ToastContainer/>
          </Container>
        </div>
  );
}

export default App;

