
import React, { useState, useEffect, useMemo } from "react";
import { axinstance } from "../services/axios";

import { useHistory } from "react-router";
import { Link } from "react-router-dom";
import { auth } from "../services/firebase";
import { Redirect } from "react-router-dom";
import { Container, Row, Col, InputGroup, Form, Button, Modal, Image, Table, Spinner } from 'react-bootstrap';
import { Field, Formik } from "formik";
import MyLoader from "./MyLoader";
import { useTable, useSortBy } from "react-table";


const ProfileInformation = (props) => {
    return (
        <Container>
            <Formik
                //validationSchema={schema}
                onSubmit={(values, actions) => {
                    actions.setSubmitting(false)
                    const formData = new FormData()
                    formData.append('userImageFile', values.file)
                    formData.append('headline', values.headline)
                    formData.append('bio', values.bio)
                    axinstance.post('user', formData).then((res) => {
                        if (res.status === 200) {
                            props.handleSetProfile(res.data.data)
                            actions.setStatus({ success: 'Updated !' })
                            props.onHide()
                        }
                    })

                }}
                initialValues={{
                    file: null,
                    headline: props.headline || '',
                    bio: props.bio || ''
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
                    actions
                }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>
                                Profile Photo
                            </Form.Label>
                            <InputGroup className="mb-3">
                                <input name="avatarFile" type="file"
                                    accept=".jpeg,.jpg,.png"
                                    onChange={(event) => {
                                        setFieldValue("file", event.currentTarget.files[0]);
                                    }} />
                            </InputGroup>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>
                                Headline
                            </Form.Label>
                            <InputGroup className="mb-3">
                                <Field
                                    as={Form.Control}
                                    name="headline"
                                />
                            </InputGroup>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>
                                Bio
                            </Form.Label>
                            <InputGroup className="mb-3">
                                <Form.Control
                                    as="textarea"
                                    name="bio"
                                    value={values.bio}
                                    onChange={handleChange}
                                />
                            </InputGroup>
                        </Form.Group>
                        <Button type="submit" disabled={isSubmitting} > Submit </Button>
                        <p className="text-success"> {status ? status.success : ''}  </p>
                    </Form>
                )}
            </Formik>
        </Container>
    )
}
const NameCreatePlaylist = (props) => {
    return (
        <Container>
            <Formik
                //validationSchema={schema}
                onSubmit={(values, actions) => {
                    actions.setSubmitting(false)
                    
                    const data = {
                        samplePlaylistName: values.samplePlaylistName,
                    }
                    axinstance.post('sampleplaylist/create', data).then((res) => {
                        if (res.status === 200) {
                            props.handleSetPlaylists(res.data.data)
                            actions.setStatus({ success: 'Updated !' })
                            props.onHide()
                        }
                    })

                }}
                initialValues={{
                    samplePlaylistName: ''
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
                    actions
                }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>
                                Sample Pack Name
                            </Form.Label>
                            <InputGroup className="mb-3">
                                <Field
                                    as={Form.Control}
                                    name="samplePlaylistName"
                                />
                            </InputGroup>
                        </Form.Group>
                        <Button type="submit" disabled={isSubmitting} > Submit </Button>
                        <p className="text-success"> {status ? status.success : ''}  </p>
                    </Form>
                )}
            </Formik>
        </Container>
    )
}

const NameCreateSamplePack = (props) => {
    return (
        <Container>
            <Formik
                //validationSchema={schema}
                onSubmit={(values, actions) => {
                    actions.setSubmitting(false)
                    
                    const data = {
                        samplePackName: values.samplePackName,
                    }
                    axinstance.post('samplepack/create', data).then((res) => {
                        if (res.status === 200) {
                            props.handleSetSamplePacks(res.data.data)
                            actions.setStatus({ success: 'Updated !' })
                            props.onHide()
                        }
                    })

                }}
                initialValues={{
                    samplePackName: ''
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
                    actions
                }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>
                                Sample Pack Name
                            </Form.Label>
                            <InputGroup className="mb-3">
                                <Field
                                    as={Form.Control}
                                    name="samplePackName"
                                />
                            </InputGroup>
                        </Form.Group>
                        <Button type="submit" disabled={isSubmitting} > Submit </Button>
                        <p className="text-success"> {status ? status.success : ''}  </p>
                    </Form>
                )}
            </Formik>
        </Container>
    )
}
const PlaylistsTable = ({playlists, handleRemove}) => {
    const columns = useMemo(
        () => [
          {
            Header: 'ID',
            accessor: 'id', // accessor is the "key" in the data
            Cell: ({cell}) => (
                <>
                <Link className="btn btn-primary" to={`sampleplaylist/edit/${cell.row.values.id}`} >View</Link>
                <Button variant="danger" onClick={() => handleRemove(cell.row.values.id)}> Remove </Button>
                </>
            )
          },
          {
            Header: 'Name',
            accessor: 'samplePlaylistName',
          },
        ],
        [playlists]
      )
   

    const playlistsTableInstance = useTable({ columns, data: playlists, initialState: { pageIndex: 0 }, },  useSortBy)
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
      } = playlistsTableInstance

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
                  <th {...column.getHeaderProps(column.getSortByToggleProps())}>
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
const SamplePacksTable = ({samplePacks, handleRemove}) => {
    // const data = React.useMemo(
    //     () => props.samplePacks,
    //     []
    //   )
 
    const columns = useMemo(
        () => [
          {
            Header: 'ID',
            accessor: 'id', // accessor is the "key" in the data
            Cell: ({cell}) => (
                <>
                <Link className="btn btn-primary" to={`samplepack/edit/${cell.row.values.id}`} >View</Link>
                <Button variant="danger" onClick={() => handleRemove(cell.row.values.id)}> Remove </Button>
                </>
            )
          },
          {
            Header: 'Name',
            accessor: 'samplePackName',
          },
          {
            Header: 'Published',
            accessor: 'published',
            Cell: ({cell}) => (
                cell.row.values.published ? <span>Yes</span> : <span>No</span>
            )
          },
          {
            Header: 'Published Date',
            accessor: 'publishedDate'
          },
          {
            Header: 'Created Date',
            accessor: 'createdDate'
          },
          {
            Header: 'Modified',
            accessor: 'updatedDate'
          }

        ],
        [samplePacks]
      )
   

    const samplePackTableInstance = useTable({ columns, data: samplePacks, initialState: { pageIndex: 0 }, },  useSortBy)
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
      } = samplePackTableInstance

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
                  <th {...column.getHeaderProps(column.getSortByToggleProps())}>
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
const Dashboard = (props) => {
    const [profile, setProfile] = useState({})
    const [samplePacks, setSamplePacks] = useState([])
    const [samplePlaylists, setSamplePlaylist] =  useState([])
    const [loading, setLoading] = useState(true)


    const [showUpdateProfile, setShowUpdateProfile] = useState(false)
    const [showCreateSamplePack, setShowCreateSamplePack] = useState(false)
    const [showCreatePlaylist, setShowCreatePlaylist] =  useState(false)
    //const history = useHistory();

    function handleSetProfile(newProfile) {
        setProfile((previousState) => Object.assign(previousState, newProfile))
    }

    function handleSetSamplePacks(newSamplePack) {
        setSamplePacks((previousState) => previousState.concat(newSamplePack))
    }

    function handleSetPlaylists(newPlaylist) {
        setSamplePlaylist((previousState) => previousState.concat(newPlaylist));
    }

    function handleRemovePack(packId) {
        setSamplePacks((previousState) => previousState.filter((e) => e.id !== packId))
        axinstance.delete(`samplepack/${packId}`);
    }

    function handleRemovePlaylist(playId) {
        setSamplePlaylist((previousState) => previousState.filter((e) => e.id !== playId))
        axinstance.delete(`sampleplaylist/${playId}`);
    }

    useEffect(() => {
        axinstance.get('user/dashboard').then((res) => {
            const results = res.data.data;
            const bio = results.bio;
            const headline = results.headline;
            const id = results.id;
            const signedUserImage = results.signedUserImage;
            const username = results.username;
            handleSetProfile({id: id, bio: bio, headline: headline, signedUserImage: signedUserImage, username: username});
            setSamplePacks(prevState => Object.assign(prevState,results.samplePacks))
            setSamplePlaylist(prevState => Object.assign(prevState, results.samplePlaylists))
            setLoading(false)
        })
    }, [])




    
 

    
    
   
    return (

        <>
            {
                loading ?
                    (
                        <MyLoader />
                    ) :
                    (
                        <Container>
                            <p className="h1 mt-4"> Dashboard </p>
                            <p className="h4"> Profile Info </p>
                            <p> Username: {profile.username}</p>
                            <p>
                                <Image className="img img-responsive img-thumbnail w-25"
                                    src={profile.signedUserImage} />
                            </p>
                            <p> Headline: {profile.headline} </p>
                            <p> Bio: {profile.bio} </p>
                            <Button type="button" onClick={() => setShowUpdateProfile(true)}>
                                Edit Info
                            </Button>

                            <Row className="mt-4">
                                <Col>
                                    <p className="h4"> Sample Packs </p>
                                    <SamplePacksTable samplePacks={samplePacks} handleRemove={handleRemovePack} />
                                    <Button type="button" onClick={() => setShowCreateSamplePack(true)}>
                                        Create Sample Pack
                                    </Button>
                                </Col>

                                <Col>
                                    <p className="h4"> Sample Playlists </p>
                                    <PlaylistsTable playlists={samplePlaylists} handleRemove={handleRemovePlaylist} />
                                    <Button type="button" onClick={() => setShowCreatePlaylist(true)}>
                                        Create Playlist
                                    </Button>
                                </Col>
                            </Row>

                            <Modal show={showUpdateProfile} onHide={() => setShowUpdateProfile(false)}>
                                <Modal.Header closeButton>
                                    <Modal.Title>Update Profile</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <ProfileInformation
                                        onHide={() => setShowUpdateProfile(false)}
                                        handleSetProfile={handleSetProfile}
                                        displayName={profile.displayName}
                                        bio={profile.bio}
                                        headline={profile.headline}
                                    />
                                </Modal.Body>
                            </Modal>

                            <Modal show={showCreateSamplePack} onHide={() => setShowCreateSamplePack(false)}>
                                <Modal.Header closeButton>
                                    <Modal.Title>Create Sample Pack</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <NameCreateSamplePack
                                        onHide={() => setShowCreateSamplePack(false)}
                                        handleSetSamplePacks={handleSetSamplePacks}
                                    />
                                </Modal.Body>
                            </Modal>

                            <Modal show={showCreatePlaylist} onHide={() => setShowCreatePlaylist(false)}>
                                <Modal.Header closeButton>
                                    <Modal.Title>Create Playlist</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <NameCreatePlaylist
                                        onHide={() => setShowCreatePlaylist(false)}
                                        handleSetPlaylists={handleSetPlaylists}
                                    />
                                </Modal.Body>
                            </Modal>

                        </Container>
                    )
            }
        </>

    )

}

export default Dashboard;