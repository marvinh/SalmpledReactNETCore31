import React, { useState, useEffect, useCallback, useMemo, useRef, useReducer, useContext } from "react";
import { axinstance } from "../services/axios";
import { useDropzone } from 'react-dropzone';
import { useParams, Link } from "react-router-dom";
import {
    useTable,
    useSortBy,
    useResizeColumns,
    useFlexLayout,
    onFetchData
} from "react-table";
import { Container, Table, Button, Spinner, Col, Row, Modal, Form, InputGroup } from "react-bootstrap";
import { Field, Formik } from "formik";
import Waveform from "./Waveform";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlusSquare } from "@fortawesome/free-solid-svg-icons";
import MyLoader from "./MyLoader";
import { AuthContext } from "../providers/AuthContext";
const SampleTable = ({ samples, handleAddToPlaylist }) => {



    const columns = useMemo(
        () => [
            {
                Header: 'Add to Playlist',
                accessor: 'id', // accessor is the "key" in the data
                width: 64,
                Cell: ({ cell, row }) => (
                    <>
                        <Button size="sm" variant="outline-dark"
                            onClick={() => {
                                handleAddToPlaylist(cell.row.values.id)
                            }}
                        ><FontAwesomeIcon icon={faPlusSquare}> </FontAwesomeIcon></Button>
                    </>
                )
            },
            {
                Header: 'File Name',
                accessor: 'fileName',
            },
            {
                Header: 'Preview',
                accessor: 'signedMP3URL',
                Cell: ({ cell, row }) => (
                    <>
                        {console.log(cell.row.values.signedMP3URL)}
                        <Waveform url={cell.row.values.signedMP3URL} />
                    </>
                )
            },

        ],
        [samples]
    )


    const sampleTableInstance = useTable({
        columns, data: samples, useControlledState: state => {
            return React.useMemo(
                () => ({
                    ...state,
                    samples: samples,
                }),
                [state, samples]
            )
        },
    },
        useResizeColumns,
        useFlexLayout)
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
        page, // Instead of using 'rows', we'll use page,
        // which has only the rows for the active page

        // The rest of these things are super handy, too ;)
        canPreviousPage,
        canNextPage,
        pageOptions,
        pageCount,
        gotoPage,
        nextPage,
        previousPage,
        setPageSize,
        state: { pageIndex, pageSize },
    } = sampleTableInstance

    return (
        // apply the table props
        <Table striped bordered hover size="sm" {...getTableProps()}>
            <thead>
                {// Loop over the header rows
                    headerGroups.map(headerGroup => (
                        // Apply the header row props
                        <tr {...headerGroup.getHeaderGroupProps()}>
                            {// Loop over the headers in each row
                                headerGroup.headers.map(column => (
                                    // Apply the header cell props
                                    <th {...column.getHeaderProps()}>
                                        {// Render the header
                                            column.render('Header')}
                                    </th>
                                ))}
                        </tr>
                    ))}
            </thead>
            {/* Apply the table body props */}
            <tbody {...getTableBodyProps()}>
                {// Loop over the table rows
                    rows.map(row => {
                        // Prepare the row for display
                        prepareRow(row)
                        return (
                            // Apply the row props
                            <tr {...row.getRowProps()}>
                                {// Loop over the rows cells
                                    row.cells.map(cell => {
                                        // Apply the cell props
                                        return (
                                            <td {...cell.getCellProps()}>
                                                {// Render the cell contents
                                                    cell.render('Cell')
                                                }
                                            </td>
                                        )
                                    })}
                            </tr>
                        )
                    })}
            </tbody>
        </Table>
    )

}

const AddToPlaylist = (props) => {
    const [loading, setLoading] = useState(true)
    const [playlists, setPlaylists] = useState([])
    useEffect(() => {
        axinstance.get('sampleplaylist/yourplaylists').then((res) => {
            const results = res.data.data;
            setPlaylists(results);
        })
    }, [])

    return (
        <Container>
            <Formik
                //validationSchema={schema}
                onSubmit={(values, actions) => {
                    actions.setSubmitting(false)

                    const data = {
                        sampleId: props.sampleId,
                        samplePlaylistId: values.samplePlaylistId,
                    }
                    axinstance.post('sampleplaylist/addsample', data).then((res) => {
                            actions.setStatus({ success: 'Updated !' })
                            props.onHide()
                    })

                }}
                initialValues={{
                    sampleId: props.sampleId,
                    samplePlaylistId: '',
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
                    dirty,
                }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>
                                Add to Playlist
                            </Form.Label>
                            <InputGroup className="mb-3">
                                {
                                    playlists.length > 0 ?
                                        <Field
                                            name="samplePlaylistId"
                                            as={Form.Select}
                                            
                                        >
                                            <option> Select Playlist</option>
                                            {
                                                
                                                playlists.map((e) => {
                                                    return (
                                                        <option key={e.id} value={e.id}> {e.samplePlaylistName} </option>
                                                    )
                                                })

                                            }
                                        </Field>
                                        : <div> Create a playlist in your dashboard first <div> <Link to="/dashboard"> Dashboard </Link> </div> </div>
                                }
                            </InputGroup>
                        </Form.Group>
                        <Button type="submit" disabled={!dirty ||isSubmitting} > Submit </Button>
                        <p className="text-success"> {status ? status.success : ''}  </p>
                    </Form>
                )}
            </Formik>
        </Container>
    )
}

const ViewSamplePack = (props) => {

    const { user, loading } = useContext(AuthContext);

    const { id } = useParams()
    const [sloading, setLoading] = useState(true)
    const [samplePack, setSamplePack] = useState({})
    const [samples, setSamples] = useState([])
    const [sampleLoading, setSampleLoading] = useState(false)
    const [playlistModal, setShowPlaylistModal] = useState(false);
    const [sampleToAdd, setSampleToAdd] = useState('');
    const [authModal, setAuthModal] = useState(false)
    useEffect(() => {
        async function fetchSamplePack() {
            axinstance.get(`samplepack/view/${id}`)
                .then((res) => {
                    setSamplePack(prevState => Object.assign(prevState, res.data.data))
                    setSamples(prevState => [...prevState, ...res.data.data.samples])
                    setLoading(false)
                })
                .catch((err) => {
                    console.log(err)
                })
        }
        fetchSamplePack()
    }, [])

    function handleAddToPlaylist(sampleId) {
        console.log(user);
        if (!!user) {
            setShowPlaylistModal(true)
            setSampleToAdd(sampleId)
        } else {
            setAuthModal(true)
        }

    }

    return (
        <Container className="pt-4">
            {sloading || loading ?
                <MyLoader /> :
                <>
                    <p> Sample Pack Name: {samplePack.samplePackName} </p>
                    <p> Created By: <Link to={`/user/view/${samplePack.username}`}> {samplePack.username} </Link> </p>
                    <Row>
                        <Col lg="12" md="12" sm="12">
                            <SampleTable
                                handleAddToPlaylist={handleAddToPlaylist}
                                samples={samples}
                            />
                        </Col>
                    </Row>
                </>
            }

            <Modal show={authModal} onHide={() => setAuthModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Auth Message</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    You are not signed in
                    <Link to="/login"> Login to Add</Link>
                </Modal.Body>
            </Modal>

            <Modal show={playlistModal} onHide={() => setShowPlaylistModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Sample To Playlist</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <AddToPlaylist
                    onHide={() => setShowPlaylistModal(false)}
                    sampleId={sampleToAdd}
                    />
                </Modal.Body>
            </Modal>
        </Container>
    )
}

export default ViewSamplePack;
