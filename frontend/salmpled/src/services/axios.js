
import axios from "axios"
import firebase from 'firebase/compat/app';
import 'firebase/compat/auth';
import { Redirect } from "react-router-dom";
import { toast } from "react-toastify";
import { myHistory } from "../index";

// export const axinstance = axios.create({ baseURL: 'https://salmpled-backend.herokuapp.com/api/v1' })

export const axinstance = axios.create({ baseURL: 'https://salmpled-backend.herokuapp.com/api/v1' })

axinstance.interceptors.response.use((response) => {
    console.log(JSON.parse(JSON.stringify(response, null, 4)));
    if (response.data.message === "Already Exists!") {
        toast('Already Exists!', {
            position: "bottom-center",
            autoClose: 2000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
        });
    }
    if (response.data.success === false) {
        myHistory.push('/err')
        throw "You may not be authorized or this resource does not exist.";
    }
    return response;
}, (error) => {
    console.log(`error ${error}`);
    //myHistory.push('/err')
    //return Promise.reject(error);
});

axinstance.interceptors.request.use(
    async (config) => {
        const currentUser = firebase.auth().currentUser
        let token = '';
        if (currentUser) {
            token = await currentUser.getIdToken()
        }
        config.headers["Authorization"] = `Bearer ${token}`
        return config
    }
)