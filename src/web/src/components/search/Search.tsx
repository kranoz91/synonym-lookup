import { Grid, List, ListItem, Typography } from "@mui/material"
import { SearchBar } from "./SearchBar"
import { useState } from "react";
import { SharedState } from "../../App";

export interface SearchProps {
    State: SharedState,
    UpdateState: (newState: SharedState) => void
}

export const Search = (Props: SearchProps) => {
    const [searchString, setSearchString] = useState('');
    const [searching, setSearching] = useState(false);

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchString(event.target.value);
    }

    function search(): Promise<string[]> {
        const headers: Headers = new Headers()
    
        headers.set('Content-Type', 'application/json')
        headers.set('Accept', 'application/json')
    
        const request: RequestInfo = new Request('https://apim-synonym-lookup-dev.azure-api.net/words/v1/words/' + searchString + '/synonyms', {
            method: 'GET',
            headers: headers
        })

        setSearching(true);
    
        return fetch(request)
            .then(res => res.json())
            .then(res => res as string[])
    }

    const handleSearch = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        search()
            .then(synonyms => {
                var newState: SharedState = {
                    LatestSearch: searchString,
                    Synonyms: synonyms
                };

                Props.UpdateState(newState);
                setSearching(false);
                setSearchString('');
            })
    }

    return (
        <Grid container>
            <Grid item xs={12}>
                <SearchBar HandleChange={handleChange} HandleSearch={handleSearch} Searching={searching} SearchString={searchString}/>
            </Grid>
            {Props.State.LatestSearch !== '' ? (
                <Grid item xs={12}>
                    <List>
                        <ListItem divider>
                            <Typography component="span">Synonyms for <Typography component="span" sx={{ fontWeight: "bold" }}>{Props.State.LatestSearch}</Typography></Typography>
                        </ListItem>
                        {Props.State.Synonyms.map((synonym, i) => {
                            return (
                                <ListItem>
                                    <Typography>{synonym}</Typography>
                                </ListItem>
                            )
                        })}
                    </List>
                </Grid>
            ) : <></>}
        </Grid>
    )
}