import React, { useState, useEffect, useMemo } from "react";
import { axinstance } from "../services/axios";

import { useHistory } from "react-router";
import { Link, useParams } from "react-router-dom";
import { auth } from "../services/firebase";
import { Redirect } from "react-router-dom";
import { Container, Row, Col, InputGroup, Form, Button, Modal, Image, Table, Spinner } from 'react-bootstrap';
import { Field, Formik } from "formik";
import MyLoader from "./MyLoader";
import { useTable, useSortBy } from "react-table";

const PlaylistsTable = ({playlists}) => {
    const columns = useMemo(
        () => [
          {
            Header: 'ID',
            accessor: 'id', // accessor is the "key" in the data
            Cell: ({cell}) => (
                <>
                <Link className="btn btn-primary" to={`/sampleplaylist/view/${cell.row.values.id}`} >View</Link>
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
                <Link className="btn btn-primary" to={`/samplepack/view/${cell.row.values.id}`} >View</Link>
                </>
            )
          },
          {
            Header: 'Name',
            accessor: 'samplePackName',
          },
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

const UserProfile = (props) => {
    const [profile, setProfile] = useState({})
    const [samplePacks, setSamplePacks] = useState([])
    const [samplePlaylists, setSamplePlaylists] =  useState([])
    const [loading, setLoading] = useState(true)
    const {username} = useParams();
    //const history = useHistory();

    function handleSetProfile(newProfile) {
        setProfile((previousState) => Object.assign(previousState, newProfile))
    }

    useEffect(() => {
        axinstance.get(`user/${username}`).then((res) => {
            const results = res.data.data;
            const bio = results.bio;
            const headline = results.headline;
            const id = results.id;
            const signedUserImage = results.signedUserImage;
            const username = results.username;
            handleSetProfile({id: id, bio: bio, headline: headline, signedUserImage: signedUserImage, username: username});
            setSamplePacks(prevState => results.samplePacks && results.samplePacks.length > 0 ? results.samplePacks : [])
            setSamplePlaylists(prevState => results.playlists && results.playlists.length > 0 ? results.playlists : [])
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
                            <p className="h1 mt-4"> User Profile </p>
                            <p className="h4"> Profile Info </p>
                            <p> Username: {profile.username}</p>
                            <p>
                                <Image className="img img-responsive img-thumbnail w-25"
                                    src={profile.signedUserImage} />
                            </p>
                            <p> Headline: {profile.headline} </p>
                            <p> Bio: {profile.bio} </p>

                            <Row className="mt-4">
                                <Col>
                                    <p className="h4"> Sample Packs </p>
                                    <SamplePacksTable samplePacks={samplePacks}/>
                                </Col>
                                <Col>
                                    <p className="h4"> Sample Playlists </p>
                                    <PlaylistsTable playlists={samplePlaylists}/>
                                </Col>
                            </Row>


                        </Container>
                    )
            }
        </>

    )

}

export default UserProfile;