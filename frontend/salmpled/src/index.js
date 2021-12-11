import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import { AuthProvider } from './providers/AuthContext';
import { Router } from 'react-router-dom';
import { createBrowserHistory } from 'history';
import { ToastContainer } from 'react-bootstrap';

export const myHistory = createBrowserHistory();

ReactDOM.render(
  <Router history={myHistory}>
    <AuthProvider>
      <App />
    </AuthProvider>
  </Router>
  ,
  document.getElementById('root')
);

