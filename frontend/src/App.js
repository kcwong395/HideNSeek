import React from 'react';
import CssBaseline from '@material-ui/core/CssBaseline';
import './App.css';
import Header from './components/Header/Header'
import Footer from './components/Footer/Footer'
import { makeStyles } from '@material-ui/core/styles';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import Hide from './components/Hide/Hide'
import Seek from './components/Seek/Seek'
import Home from './components/Home/Home'

const useStyles = makeStyles((theme) => ({
  layout: {
    display: 'flex',
    flexDirection: 'column',
    minHeight: '100vh'
  }
}));

export default function App() {

  const classes = useStyles();
  return (
    <div className={classes.layout}>
      <Router>
        <CssBaseline />
        <Header />
        <Switch>
          <Route path="/">
            <Home />
          </Route>
          <Route path="/hide">
            <Hide />
          </Route>
          <Route path="/seek">
            <Seek />
          </Route>
        </Switch>
        <Footer />
      </Router>
    </div>
  );
}
