import React from 'react';
import './App.css';
import Typography from '@mui/material/Typography';
import { Container } from '@mui/material';
import { useState } from 'react';
import { SearchBar } from './components/search/SearchBar';



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

  return (
    <Container>
      <SearchBar HandleChange={handleChange} HandleSearch={handleSearch}/>
      <Typography>{onClickSearchString}</Typography>
    </Container>
  );
}

export default App;
