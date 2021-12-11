import firebase from 'firebase/compat/app';
import 'firebase/compat/auth';
import { axinstance } from './axios';
// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional

const firebaseConfig = {
    apiKey: process.env.REACT_APP_apiKey,
    authDomain: process.env.REACT_APP_authDomain,
    projectId: process.env.REACT_APP_projectId,
    storageBucket: process.env.REACT_APP_storageBucket,
    messagingSenderId: process.env.REACT_APP_messagingSenderId,
    appId: process.env.REACT_APP_appId,
    measurementId: process.env.REACT_APP_measurementId
};

const app = firebase.initializeApp(firebaseConfig)
const auth = app.auth();
const googleProvider = new firebase.auth.GoogleAuthProvider();

const signInWithGoogle = async () => {

    try {
        const res = await auth.signInWithPopup(googleProvider).then(async (res) => {
            await axinstance.post('user/createFromGoogle');
        })
    } catch (err){
        console.log(err)
    }
};


const logout = () => {
    console.log("signout test")
    auth.signOut();
};

export {
    auth,
    googleProvider,
    signInWithGoogle,
    logout,
};