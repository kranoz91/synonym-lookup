import React from 'react';
import './styles/App.css';
import Typography from '@mui/material/Typography';
import { Container, Grid } from '@mui/material';
import { useState } from 'react';
import { SearchBar } from './components/search/SearchBar';
import { Paper } from '@mui/material';
import { MsalProvider, AuthenticatedTemplate, UnauthenticatedTemplate } from '@azure/msal-react';
import { IPublicClientApplication } from '@azure/msal-browser';
import { PageLayout } from './components/layout/PageLayout';
import { CreateWord } from './components/admin/CreateWord';

const MainContent = () => {
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

  return (
    <Container className="App" sx={{ mt: 4, mb: 4 }}>
      <Grid container maxWidth="lg" spacing={3}>
        <AuthenticatedTemplate>
          <Grid item xs={12} md={6} lg={6}>
            <Paper sx={{ p: 2, height: 240 }}>
              <CreateWord />
            </Paper>
          </Grid>
          <Grid item xs={12} md={6} lg={6}>
            <Paper sx={{ p: 2, height: 240 }}>
              <SearchBar HandleChange={handleChange} HandleSearch={handleSearch}/>
              <Typography>{JSON.stringify(synonyms)}</Typography>
            </Paper>
          </Grid>
        </AuthenticatedTemplate>
        <UnauthenticatedTemplate>
        <Grid item xs={12} md={12} lg={12}>
            <Paper sx={{ p: 2, height: 240 }}>
              <SearchBar HandleChange={handleChange} HandleSearch={handleSearch}/>
              <Typography>{JSON.stringify(synonyms)}</Typography>
            </Paper>
          </Grid>
        </UnauthenticatedTemplate>
      </Grid>
    </Container>
  )
}

interface AppProps {
  Instance: IPublicClientApplication
}

const App = (Props: AppProps) => {
  return (
    <MsalProvider instance={Props.Instance}>
      <PageLayout>
        <MainContent />
      </PageLayout>
    </MsalProvider>
  );
}

export default App;
