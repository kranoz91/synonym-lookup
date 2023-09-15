import React from 'react';
import './App.css';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { Grid, ThemeProvider, createTheme } from '@mui/material';
import { Container } from '@mui/material';
import { useState } from 'react';
import { SearchBar } from './components/search/SearchBar';
import { Paper } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';

function App() {
  const [searchString, setSearchString] = useState('');

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchString(event.target.value);
  }

  const [onClickSearchString, setOnClickSearchString] = useState('');

  const handleSearch = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
    setOnClickSearchString(searchString);
  }

  const defaultTheme = createTheme();

  return (
    <ThemeProvider theme={defaultTheme}>
      <Box sx={{ display: 'flex' }}>
        <CssBaseline />
        <Box
          component="main"
          sx={{
            backgroundColor: (theme) =>
              theme.palette.mode === 'light'
                ? theme.palette.grey[100]
                : theme.palette.grey[900],
            flexGrow: 1,
            height: '100vh',
            overflow: 'auto',
            alignContent: 'center'
          }}
        >
          <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            <Grid container spacing={3} sx={{ flexDirection: 'column', alignContent: 'center' }}>
              <Grid item xs={12} md={8} lg={9}>
                <Paper
                  sx={{
                    p: 2,
                    display: 'flex',
                    flexDirection: 'column',
                    height: 240,
                  }}
                >
                  <SearchBar HandleChange={handleChange} HandleSearch={handleSearch}/>
                  <Typography>{onClickSearchString}</Typography>
                </Paper>
              </Grid>
            </Grid>
          </Container>
        </Box>
      </Box>
    </ThemeProvider>
  );
}

export default App;
