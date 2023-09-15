import React from 'react';
import './App.css';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { Grid, ThemeProvider, createTheme } from '@mui/material';
import { useState } from 'react';
import { SearchBar } from './components/search/SearchBar';
import { Paper } from '@mui/material';

function App() {
  const [searchString, setSearchString] = useState('');

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchString(event.target.value);
  }

  const [synonyms, setSynonyms] = useState(['']);

  function search(): Promise<string[]> {
    const headers: Headers = new Headers()

    headers.set('Content-Type', 'application/json')
    headers.set('Accept', 'application/json')

    const request: RequestInfo = new Request('https://apim-synonym-lookup-dev.azure-api.net/words/v1/words/' + searchString + '/synonyms', {
      method: 'GET',
      headers: headers
    })

    return fetch(request)
      .then(res => res.json())
      .then(res => res as string[])
  }

  const handleSearch = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
    search()
      .then(synonyms => {
        setSynonyms(synonyms);
      })
  }

  const defaultTheme = createTheme();

  return (
    <ThemeProvider theme={defaultTheme}>
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
        }}
      >
          <Grid container spacing={3} sx={{ mt: 4, mb: 4, flexDirection: 'column', alignContent: 'center' }}>
            <Grid item xs={12} md={8} lg={9}>
              <Paper sx={{ p: 2, height: 240 }}>
                <SearchBar HandleChange={handleChange} HandleSearch={handleSearch}/>
                <Typography>{JSON.stringify(synonyms)}</Typography>
              </Paper>
            </Grid>
          </Grid>
      </Box>
    </ThemeProvider>
  );
}

export default App;
