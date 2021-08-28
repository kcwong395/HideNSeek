import React from 'react';
import { Container } from '@material-ui/core'
import Banner from './Banner/Banner';
import { makeStyles } from '@material-ui/core/styles';

export default function Home() {

    return (
        <React.Fragment>
            <Container maxWidth="lg" style={{ textAlign: 'center' }}>
                <Banner />
            </Container>
        </React.Fragment>
    )
}