import React, { useState, useEffect, useCallback, useMemo, useRef, useReducer } from "react";
import { axinstance } from "../services/axios";
import { useDropzone } from 'react-dropzone';
import { useParams, Link, Redirect } from "react-router-dom";
import {
  useTable,
  useSortBy,
  useResizeColumns,
  useFlexLayout,
  onFetchData
} from "react-table";
import { Container, Table, Button, Spinner, Col, Row } from "react-bootstrap";
import Waveform from "./Waveform";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenAlt, faPencilAlt, faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import MyLoader from "./MyLoader";
function SampleDropzone(props) {

  const onDrop = useCallback(acceptedFiles => {
    // Do something with the files
    props.handleOnDrop(acceptedFiles);

  }, [])
  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop, accept: 'audio/wav, audio/aif, audio/aiff' })

  return (
    <div {...getRootProps()}>
      <input {...getInputProps()} />
      {
        <Container className="border text-center" style={{ height: "25vh" }}>
          <p> Drag and drop .wav or .aif/.aiff files here or click to select files. </p>
        </Container>
      }
    </div>
  )
}

const SampleTable = ({ samples, handleSampleRemove }) => {



  const columns = useMemo(
    () => [
      {
        Header: 'Actions',
        accessor: 'id', // accessor is the "key" in the data
        width: 64,
        Cell: ({ cell, row }) => (
          <>
            {/* <Link
              className="btn btn-sm btn-outline-success"
              to={`/sample/edit/${cell.row.values.id}`} ><FontAwesomeIcon icon={faPencilAlt}> </FontAwesomeIcon></Link> */}
            <Button size="sm" variant="outline-danger"
              onClick={() => {
                handleSampleRemove(cell.row.values.id)
              }}
            ><FontAwesomeIcon icon={faTrashAlt}> </FontAwesomeIcon></Button>
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


  const sampleTableInstance = useTable({ columns, data: samples, useControlledState: state => {
    return React.useMemo(
      () => ({
        ...state,
        samples: samples,
      }),
      [state, samples]
    )
  }, },
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

const EditSamplePack = (props) => {
  const { id } = useParams()
  const [loading, setLoading] = useState(true)
  const [samplePack, setSamplePack] = useState({})
  const [samples, setSamples] = useState([])
  const [sampleLoading, setSampleLoading] = useState(false)
  const [disableToggle, setDisableToggle] = useState(false)
  const [failed, setFailed] = useState(false)
  const handleOnDrop = (acceptedFiles) => {
    console.log(acceptedFiles);
    console.log("samples", samples)
    setSampleLoading(true)
    const formData = new FormData();
    // Update the formData object
    acceptedFiles.forEach((e) => {
      formData.append(
        "files",
        e,
      );
    })

    formData.append(
      "SamplePackId",
      id
    )

    axinstance.post('/sample/uploadsamples', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    }).then((res) => {
      setSamples(prevState => [...prevState, ...res.data.data])
      setSampleLoading(false)
    }).catch((err) => {
      console.log(err);
      setSampleLoading(false)
    })

  }

  
  useEffect(() => {
    async function fetchSamplePack() {
      axinstance.get(`samplepack/edit/${id}`)
        .then((res) => {
          
          setSamplePack(prevState=> Object.assign(prevState,res.data.data))
          setSamples(prevState => [...prevState, ...res.data.data.samples])
          setLoading(false)
        })
        .catch((err) => {
          console.log(err)
        })
    }
    fetchSamplePack()
  }, [])


  const handleSampleRemove = (id) => {
    setDisableToggle(true)
    const data = samples.data
    setSamples((prevState) => prevState.filter((e) => (e.id !== id)))
    axinstance.delete(`sample/${id}`).then((res) => {
      setDisableToggle(false)
    }).catch((err) => {
      setDisableToggle(false)
    });
  }

  return (
    <Container className="pt-4">
      {loading || sampleLoading ?
        <MyLoader /> :
        <>
        <p> Sample Pack Name: {samplePack.samplePackName} </p>
        {/* <p> Sample Pack Genres: </p>
        {
          samplePack.samplePackSamplePackGenres
        } */}
        <Row>
          <Col lg="12" md="12" sm="12">
            <SampleDropzone
              handleOnDrop={handleOnDrop}
            />
          </Col>
          <Col lg="12" md="12" sm="12">
           <SampleTable 
           samples={samples}
           handleSampleRemove={handleSampleRemove}
           />
          </Col>
        </Row>
        </>
      }
    </Container>
  )
}


export default EditSamplePack;
