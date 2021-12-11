import React, { useEffect, useState } from "react";
import { Redirect, useHistory } from "react-router-dom";
import { auth } from "../../services/firebase";

import { Formik, Field } from "formik";
import * as yup from 'yup'
import { axinstance } from "../../services/axios";

import { Container, Form, InputGroup, Button } from "react-bootstrap";
import axios from "axios";
import MyLoader from "../MyLoader";
const schema = yup.object({
    username: yup.string()
        .required('Username is required')
        .matches(/^(?=[a-zA-Z0-9._]{8,20}$)(?!.*[_.]{2})[^_.].*[^_.]$/, "Username Format is not correct!")
        .test('Unique Username', 'Username already in use', // <- key, message
            function (value) {
                return new Promise((resolve, reject) => {
                    axinstance.get(`user/${value}/avail`).then((res) => {
                        // exists
                        if (res.data === true) {
                            resolve(true)
                        } else {
                            resolve(false)
                        }
                    })
                })
            })
});

const SetUsername = ({ username }) => {
    const [currentUser, setCurrentUser] = useState(null);
    const history = useHistory();
    const [loading, setLoading] = useState(true)
    
    useEffect(() => {
        if(!!username)
        {
            return <Redirect to="/dashboard" />
        }else{
            setLoading(false)
        }
    }, [])

    return (

        <Container>
            {loading ?
                <MyLoader>
                </MyLoader>
                :
                <>
                    <h1> Set Your Username </h1>
                    <Formik
                        validationSchema={schema}
                        onSubmit={(values, actions) => {
                            const { username } = values;
                            actions.setSubmitting(false)
                            const data = {
                                username: username
                            }
                            axinstance.post('user/setusername', data)
                                .then((res) => {
                                    if (res.data.success) {
                                        history.push('/login');
                                    } else {
                                        history.push('/somethinghappend');
                                    }
                                })

                        }}
                        initialValues={{
                            username: '',
                        }
                        }
                    >
                        {({
                            handleSubmit,
                            handleChange,
                            handleBlur,
                            values,
                            setFieldValue,
                            touched,
                            isValid,
                            errors,
                            isSubmitting,
                            status,
                            actions,
                            dirty
                        }) => (
                            <Form noValidate onSubmit={handleSubmit}>
                                <Form.Group className="mb-3">
                                    <Form.Label>
                                        Username
                                    </Form.Label>
                                    <InputGroup className="mb-3">
                                        <Field
                                            as={Form.Control}
                                            name="username"
                                            isValid={touched.username && !errors.username}
                                            isInvalid={!!errors.username}
                                        />
                                        <Form.Control.Feedback type="invalid">
                                            {errors.username}
                                        </Form.Control.Feedback>
                                    </InputGroup>
                                </Form.Group>
                                <Button type="submit" disabled={isSubmitting || !dirty} > Submit </Button>
                                {
                                    status && status.error && (
                                        <p className="text-danger">{status.message}</p>
                                    )}
                                {
                                    status && !status.error && (
                                        <p className="text-success">{status.message}</p>
                                    )
                                }
                            </Form>
                        )}
                    </Formik>
                </>
            }
        </Container>

    )

};

export default SetUsername;