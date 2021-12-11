import { faPlayCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useRef, useState } from "react";
import { Container, Button, Row, Col } from "react-bootstrap";
import WaveSurfer from "wavesurfer.js";

const formWaveSurferOptions = ref => ({
    container: ref,
    waveColor: "#000",
    progressColor: "#DA4E4D",
    cursorColor: "#DA4E4D",
    barWidth: 1,
    height: 24,
    responsive: true,
    backend: 'MediaElement',
    // If true, normalize by the maximum peak instead of 1.0.
    normalize: false,
    // Use the PeakCache to improve rendering speed of large waveforms.
    partialRender: false,
    splitChannels: false,
});

export default function Waveform({ url, id }) {
    const waveformRef = useRef(null);
    const wavesurfer = useRef(null);
    const [playing, setPlay] = useState(false);

    // create new WaveSurfer instance
    // On component mount and when url changes
    useEffect(() => {

        const options = formWaveSurferOptions(waveformRef.current);
        console.log(waveformRef.current);
        wavesurfer.current = WaveSurfer.create(options);

        wavesurfer.current.load(url);

        wavesurfer.current.on("ready", function () {
            // https://wavesurfer-js.org/docs/methods.html
            // wavesurfer.current.play();
            //setPlay(true);
            wavesurfer.current.setVolume(0.125);
            // make sure object stillavailable when file loaded
            if (wavesurfer.current) {
                wavesurfer.current.drawBuffer();
            }
        });

        wavesurfer.current.on('error', function(err) {
            if (wavesurfer.current) {
                console.log("err", err);
            }
        })
        wavesurfer.current.on("finish", function () {
            if (wavesurfer.current) {
                console.log("finish", playing);
            }

        })

        // Removes events, elements and disconnects Web Audio nodes.
        // when component unmount
        return () => wavesurfer.current.destroy();
    }, [url]);

    const handlePlayPause = () => {
        wavesurfer.current.playPause();
        setPlay(wavesurfer.current.isPlaying())
    };

    return (
        <Row className="text-center">
            <Col lg="2">
                <Button variant="dark" size="sm" onClick={handlePlayPause}>
                    <FontAwesomeIcon size="sm" icon={faPlayCircle}></FontAwesomeIcon>
                </Button>
            </Col>
            <Col lg="10">
                <div id={`waveform-${id}`} ref={waveformRef} />
            </Col>
        </Row>
    );
}
