import { useState } from 'react';
import { TextField, Button, List, ListItem, ListItemText, ListItemSecondaryAction, IconButton, Typography, Paper, Box, CircularProgress } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import { Error } from '../../problemDetails';
import AddIcon from '@mui/icons-material/Add';

export interface Word {
  word: string;
  synonyms: string[];
}

interface AddWordProps {
  onAddWord: (word: Word) => void,
  error: Error|undefined,
  creating: boolean
}

const AddWord = ({ onAddWord, error, creating }: AddWordProps) => {
  const [newWord, setNewWord] = useState<string>('');
  const [synonyms, setSynonyms] = useState<string>('');
  const [synonymsList, setSynonymsList] = useState<string[]>([]);

  const handleAddSynonym = () => {
    if (synonyms.trim() !== '') {
      setSynonymsList([...synonymsList, synonyms]);
      setSynonyms('');
    }
  };

  const handleRemoveSynonym = (synonym: string) => {
    const updatedSynonyms = synonymsList.filter((s) => s !== synonym);
    setSynonymsList(updatedSynonyms);
  };

  const handleAddWord = () => {
    const wordData: Word = {
      word: newWord,
      synonyms: synonymsList,
    };

    onAddWord(wordData);

    // Clear input fields
    setNewWord('');
    setSynonyms('');
    setSynonymsList([]);
  };

  return (
    <Paper elevation={3} sx={{ padding: '16px' }}>
      <Typography variant="h5">Add a New Word</Typography>
      {error !== undefined ? (
        <Typography sx={{color: 'red'}}>{error.message}</Typography>
      ): <></>}
      <TextField
            label="Word"
            variant="outlined"
            fullWidth
            margin="normal"
            value={newWord}
            onChange={(e) => setNewWord(e.target.value)}
        />
        <Box style={{ display: 'flex', alignItems: 'center' }}>
            <TextField
                label="Synonyms"
                variant="outlined"
                fullWidth
                margin="normal"
                value={synonyms}
                onChange={(e) => setSynonyms(e.target.value)}
            />
            <Button
                color="inherit"
                onClick={handleAddSynonym}
                sx={{ marginLeft: '8px', marginBottom: '8px', marginTop: '16px', height: '100%', fontSize: '0.78rem' }}
            >
                <AddIcon />
            </Button>
        </Box>
      <List>
        {synonymsList.map((synonym, index) => (
          <ListItem key={index}>
            <ListItemText primary={synonym} />
            <ListItemSecondaryAction>
              <IconButton
                edge="end"
                aria-label="delete"
                onClick={() => handleRemoveSynonym(synonym)}
              >
                <DeleteIcon />
              </IconButton>
            </ListItemSecondaryAction>
          </ListItem>
        ))}
      </List>
        {creating ? (
            <CircularProgress sx={{ height: '2.5rem'}}/>
          ) : (
            <Button
              variant="contained"
              color="primary"
              onClick={handleAddWord}
              fullWidth
              sx={{ height: '2.5rem'}}
            >
              Submit
            </Button>
          )
        }
    </Paper>
  );
};

export default AddWord;
