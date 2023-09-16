import React from 'react';
import './App.css';
import Typography from '@mui/material/Typography';
import { Grid } from '@mui/material';
import { useState } from 'react';
import { SearchBar } from './components/search/SearchBar';
import { Paper } from '@mui/material';
import { MsalProvider, AuthenticatedTemplate, useMsal, UnauthenticatedTemplate } from '@azure/msal-react';
import { IPublicClientApplication } from '@azure/msal-browser';
import { PageLayout } from './components/layout/PageLayout';
import { Login } from './components/login/Login';

const MainContent = () => {
  const {instance} = useMsal();
  const activeAccount = instance.getActiveAccount();

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
    <div className="App">
      <AuthenticatedTemplate>
        <Grid container spacing={3} sx={{ mt: 4, mb: 4, flexDirection: 'column', alignContent: 'center' }}>
          <Grid item xs={12} md={8} lg={9}>
            <Paper sx={{ p: 2, height: 240 }}>
              <SearchBar HandleChange={handleChange} HandleSearch={handleSearch}/>
              <Typography>{JSON.stringify(synonyms)}</Typography>
            </Paper>
          </Grid>
        </Grid>
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <Login />
      </UnauthenticatedTemplate>
    </div>
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
