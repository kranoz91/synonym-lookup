import { styled } from '@mui/material/styles';
import SearchIcon from '@mui/icons-material/Search';
import CircularProgress from '@mui/material/CircularProgress';
import * as React from 'react';
import { Button } from '@mui/material';
import { Form } from 'react-bootstrap';

const SearchIconWrapper = styled('div')(({ theme }) => ({
    height: '100%',
    position: 'absolute',
    display: '-webkit-inline-box',
    alignItems: 'center',
    justifyContent: 'center',
}));

export interface SearchBarProps {
    HandleChange: (event: React.ChangeEvent<HTMLInputElement>) => void,
    HandleSearch: (event: React.MouseEvent<HTMLButtonElement>) => void,
    Searching: boolean
}

export const SearchBar = (Props: SearchBarProps) => {
    return (
        <Form style={{ display: "flex" }}>
            <Form.Control
                type="search"
                placeholder="Search..."
                className="me-2"
                aria-label="Search" />
            {Props.Searching ? (
                <CircularProgress />
            ) : (
                <Button type="submit" onClick={Props.HandleSearch} sx={{height: 39}}>
                    <SearchIconWrapper>
                        <SearchIcon />
                    </SearchIconWrapper>
                </Button>
            )}
        </Form>
    );
}