import React from 'react';
import { Container } from '@material-ui/core'
import Banner from './Banner/Banner';

export default function Home() {

    return (
        <React.Fragment>
            <Container maxWidth="lg" style={{ textAlign: 'center' }}>
                <Banner />
            </Container>
            <Container maxWidth="lg" style={{ textAlign: 'center' }}>
            <video width="80%" height="80%" controls>
                <source src="HideNSeek-Demo.mkv" type="video/mp4" />
                <source src="HideNSeek-Demo.webm" type="video/webm" />
                Your browser does not support the video tag.
            </video> 
            </Container>
        </React.Fragment>
    )
}