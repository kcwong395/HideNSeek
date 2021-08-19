import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import Hide from '../Hide/Hide'
import Seek from '../Seek/Seek'
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    minHeight: '100vh'
  }
}));

export default function Main() {
  
  const classes = useStyles();

  return (
    <React.Fragment className={classes.root}>
      <p>test</p>
      <Router>
        <Switch>
          <Route path="/">
            <Hide />
          </Route>
          <Route path="/seek">
            <Seek />
          </Route>
        </Switch>
      </Router>
    </React.Fragment>
  );
}