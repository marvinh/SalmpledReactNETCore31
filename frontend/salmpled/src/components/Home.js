import React, { useEffect, useState } from "react"
import { Container, Button, Table } from "react-bootstrap"
import { axinstance } from "../services/axios"
import MyLoader from "./MyLoader"
import { Link } from "react-router-dom"
const Home = () => {
    const [samplePacks, setSamplePacks] = useState([])
    const [samplePlaylists, setSamplePlaylists] = useState([])
    const [packsLoading, setPacksLoading] = useState(true)
    const [playlistsLoading, setPlaylistsLoading] = useState(true)
    const [displayToggle, setDisableToggle] = useState(false)
    useEffect(() => {
        axinstance.get(`sampleplaylist/all`).then((res) => {
            const results = res.data.data;
            setSamplePlaylists((prevState) => results)
            setPlaylistsLoading(false)
        })
    }, [])

    useEffect(() => {
        axinstance.get(`samplepack/all`).then((res) => {
            const results = res.data.data;
            setSamplePacks((prevState) => results)
            setPacksLoading(false)
        })
    }, [])

    return (
        <Container>
            <p> Welcome Home Browse Sample Packs Or Playlists</p>
            <p> Packs are collections of user audio files </p>
            <p> Playlists are collections of audio files from various packs</p>
            <p className="h4"> To display on the homepage add at least one sample to a pack or playlist</p>
            <p className="h4"> {!displayToggle ? "You are now viewing Packs" : "You are now viewing Playlists"}</p>
            <Button onClick={() => setDisableToggle(!displayToggle)}> {!displayToggle ? "View Playlists" : "View Packs"}</Button>
            {
                !displayToggle ?
                    packsLoading ?
                        <MyLoader />
                        :
                        <Table>
                        <thead>
                            <tr>
                                <td>
                                    Name
                                </td>
                                <td>
                                    View Playlist
                                </td>
                                <td>
                                    View User Profile
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            {

                                samplePacks.map((ele) => {
                                    return (

                                        <tr key={ele.id}>
                                            <td> Name: {ele.samplePackName}</td>
                                            <td> View: <Link to={`samplepack/view/${ele.id}`} > View Pack Contents </Link></td>
                                            <td> User: <Link to={`user/view/${ele.username}`}> {ele.username} </Link></td>
                                        </tr>
                                    )
                                })
                            }
                        </tbody>
                    </Table>
                    :
                    playlistsLoading ?
                        <MyLoader />
                        :
                        <Table>
                            <thead>
                                <tr>
                                    <td>
                                        Name
                                    </td>
                                    <td>
                                        View Playlist
                                    </td>
                                    <td>
                                        View User Profile
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                {

                                    samplePlaylists.map((ele) => {
                                        return (

                                            <tr key={ele.id}>
                                                <td> Name: {ele.samplePlaylistName}</td>
                                                <td> View: <Link to={`sampleplaylist/view/${ele.id}`} > View Playlist Contents </Link></td>
                                                <td> User: <Link to={`user/view/${ele.username}`}> {ele.username} </Link></td>
                                            </tr>
                                        )
                                    })
                                }
                            </tbody>
                        </Table>
            }
        </Container>
    )
}

export default Home;