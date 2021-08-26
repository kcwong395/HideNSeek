import React from 'react';
import Typography from '@material-ui/core/Typography';
import AppBar from '@material-ui/core/AppBar'
import Toolbar from '@material-ui/core/Toolbar'
import { makeStyles } from '@material-ui/core/styles';
import Link from '@material-ui/core/Link'
import { Link as RouterLink } from 'react-router-dom';

const useStyles = makeStyles((theme) => ({
  appBar: {
    position: 'relative',
    borderBottom: `1px solid ${theme.palette.divider}`,
  },
  // TODO: Make diff padding for diff devices, or make colleapse menu for mobile
  toolbar: {
    flexWrap: 'wrap',
    padding: '0px 10%'
  },
  toolbarTitle: {
    flexGrow: 1,
  },
  link: {
    margin: theme.spacing(1, 1.5),
  },
}));

export default function Header() {

  const classes = useStyles();

  return (
    <React.Fragment>
      <AppBar position="absolute" color="default" elevation={0} className={classes.appBar}>
        <Toolbar className={classes.toolbar}>
          <Typography variant="h6" color="inherit" noWrap className={classes.toolbarTitle}>
            <RouterLink to="/" style={{ textDecoration: 'none', color: '#202020' }}>HideNSeek</RouterLink>
          </Typography>
          <nav>
            <Link variant="button" color="textPrimary" href="/" className={classes.link}>
              Hide
            </Link>
            <Link variant="button" color="textPrimary" href="/seek" className={classes.link}>
              Seek
            </Link>
          </nav>
        </Toolbar>
      </AppBar>
    </React.Fragment>
  );
}