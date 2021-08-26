import React, {useState} from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import Checkbox from '@material-ui/core/Checkbox';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import { FormControlLabel } from '@material-ui/core';
import Dropzone from '../Dropzone/Dropzone';
import axios from 'axios'


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

export default function Seek() {

  const classes = useStyles();

  const [selectedFile, setSelectedFile] = useState(null);

  function handleSubmit(e) {
    
    e.preventDefault();
    
    const formData = new FormData()
    formData.append('image', selectedFile)

    axios.post("http://localhost:5000/api/seek", formData)
    .then(resp => {
      console.log(resp)
      const downloadUrl = window.URL.createObjectURL(new Blob([resp.data.message]));
        const link = document.createElement('a');
        link.href = downloadUrl;
        link.setAttribute('download', 'message.txt'); //any other extension
        document.body.appendChild(link);
        link.click();
        link.remove();
    });
  }

  return (
    <Container component="main" maxWidth="xs">
      <div className={classes.paper}>
        <Avatar className={classes.avatar}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h6">
          Extract Your Secret
        </Typography>
        <form 
          action=""
          onSubmit={handleSubmit}
          className={classes.form} 
          noValidate
        >
          <Typography variant="subtitle2" gutterBottom>
            Altered Image:
          </Typography>
          <Dropzone onFileChange={ setSelectedFile } />
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