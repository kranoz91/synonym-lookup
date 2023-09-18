import { Button, Container, Grid, IconButton, List, ListItem, TextField, Typography } from "@mui/material"
import DeleteIcon from '@mui/icons-material/Delete';
import { useState } from "react";

interface Word {
    value: string,
    synonyms: string[]
}

export interface CreateWordProps {
    LocationCallback: (location: string) => void
}

export const CreateWord = (Props: CreateWordProps) => {
    function create(word: Word): Promise<string> {
        const headers: Headers = new Headers()
    
        headers.set('Content-Type', 'application/json')
        headers.set('Accept', 'application/json')
    
        const request: RequestInfo = new Request('https://apim-synonym-lookup-dev.azure-api.net/words/v1/words/', {
          method: 'POST',
          body: JSON.stringify(word),
          headers: headers
        })
    
        return fetch(request)
          .then(res => res.headers.get("Location") ?? "")
    }
    
    const handleCreate = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();

        var wordToCreate: Word = {
            value: word,
            synonyms: synonyms
        };

        create(wordToCreate)
          .then(location => {
            Props.LocationCallback(location);
          })
    }

    var [word, setWord] = useState("");
    var [synonyms, setSynonyms] = useState([""]);

    const updateSynonym = (event: React.ChangeEvent<HTMLInputElement>) => {
        var newSynonyms = synonyms.concat(event.target.value);
        setSynonyms(newSynonyms);
    }

    return (
        <Container>
            <form>
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <Typography>Create word</Typography>
                    </Grid>
                    <Grid item xs={12} sx={{ display:"flex", flexDirection: "column" }}>
                        <TextField variant="outlined" helperText="Word" onChange={(e) => setWord(e.target.value)}/>
                    </Grid>
                    <Grid item xs={12} sx={{ display:"flex", flexDirection: "column" }}>
                        <Typography>Synonyms</Typography>
                        <List>
                            <ListItem
                                sx={{ paddingLeft:0 }}
                                secondaryAction={
                                    <IconButton edge="end" aria-label="delete">
                                        <DeleteIcon />
                                    </IconButton>
                                }>
                                <TextField sx={{ width: 1 }} variant="outlined" onChange={updateSynonym}/>
                            </ListItem>
                        </List>
                    </Grid>
                    <Grid item xs={12}>
                        <Button onClick={handleCreate}>Create</Button>
                    </Grid>
                </Grid>
            </form>
        </Container>
    )
}