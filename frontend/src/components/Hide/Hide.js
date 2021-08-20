import React, {useCallback} from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import Checkbox from '@material-ui/core/Checkbox';
import { TextareaAutosize } from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import { FormControlLabel } from '@material-ui/core';
import Dropzone from './Dropzone/Dropzone';

const useStyles = makeStyles((theme) => ({
  paper: {
    marginTop: theme.spacing(8),
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  avatar: {
    margin: theme.spacing(1),
    backgroundColor: theme.palette.secondary.main,
  },
  secret: {
    width: '100%',
  },
  form: {
    width: '100%', // Fix IE 11 issue.
    marginTop: theme.spacing(1),
  },
  submit: {
    margin: theme.spacing(3, 0, 2),
  },
}));

export default function Hide() {

  const classes = useStyles();

  return (
    <Container component="main" maxWidth="xs">
      <CssBaseline />
      <div className={classes.paper}>
        <Avatar className={classes.avatar}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h6">
          Insert Your Secret
        </Typography>
        <form className={classes.form} noValidate>
          <Typography variant="subtitle2" gutterBottom>
            Base Image File:
          </Typography>
          <Dropzone />
          <Typography variant="subtitle2" gutterBottom>
            Message to be inserted:
          </Typography>
          <TextareaAutosize
            minRows={5}
            maxRows={5}
            placeholder="Insert your message here"
            className={classes.secret}
          />
          <FormControlLabel
            control={<Checkbox value="isAdvance" color="primary" />}
            label="Advance Options"
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            color="primary"
            className={classes.submit}
          >
            Submit
          </Button>
        </form>
      </div>
    </Container>
  );
}