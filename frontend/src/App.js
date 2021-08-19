import React from 'react';
import CssBaseline from '@material-ui/core/CssBaseline';
import './App.css';
import Header from './components/Header/Header'
import Footer from './components/Footer/Footer'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import Hide from './components/Hide/Hide'
import Seek from './components/Seek/Seek'
import Main from './components/Main/Main'

export default function App() {
  return (
    <React.Fragment>
      <CssBaseline />
      <Header />
      <Main />
      <Footer />
    </React.Fragment>
  );
}
