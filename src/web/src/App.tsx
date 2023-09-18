import './styles/App.css';
import { Container, Grid } from '@mui/material';
import { useState } from 'react';
import { Paper } from '@mui/material';
import { MsalProvider, AuthenticatedTemplate, UnauthenticatedTemplate } from '@azure/msal-react';
import { IPublicClientApplication } from '@azure/msal-browser';
import { PageLayout } from './components/layout/PageLayout';
import { CreateWord } from './components/admin/CreateWord';
import { Search } from './components/search/Search';

const MainContent = () => {
  const [synonyms, setSynonyms] = useState(['']);

  function updateSynonyms(synonyms: string[]) {
      setSynonyms(synonyms);
  }

  return (
    <Container className="App" sx={{ mt: 4, mb: 4 }}>
      <Grid container maxWidth="lg" spacing={3} sx={{justifyContent: "center"}}>
        <AuthenticatedTemplate>
          <Grid item xs={12} md={6} lg={6}>
            <Paper sx={{ p: 2 }}>
              <CreateWord Callback={updateSynonyms}/>
            </Paper>
          </Grid>
          <Grid item xs={12} md={6} lg={6}>
            <Paper sx={{ p: 2 }}>
              <Search Synonyms={synonyms} Callback={updateSynonyms}/>
            </Paper>
          </Grid>
        </AuthenticatedTemplate>
        <UnauthenticatedTemplate>
        <Grid item xs={8}>
            <Paper sx={{ p: 2 }}>
              <Search Synonyms={synonyms} Callback={updateSynonyms}/>
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
