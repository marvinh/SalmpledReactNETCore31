import React, { useEffect, useState } from "react";
import { axinstance } from "../services/axios";
import { auth } from "../services/firebase";
export const AuthContext = React.createContext();

//3.
export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [hasUsername, setHasUsername] = useState(false);
  useEffect(() => {
    const unsubscribe = auth.onAuthStateChanged(user => {
      setUser((previousState) => { 
        return user;
      })
      setLoading(false)
    })
    
    return unsubscribe;
  }, []);
  
  return (
      <AuthContext.Provider value={{ user, loading}}>{ children}</AuthContext.Provider>
  );
};