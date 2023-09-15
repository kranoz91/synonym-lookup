import { styled, alpha } from '@mui/material/styles';
import SearchIcon from '@mui/icons-material/Search';
import InputBase from '@mui/material/InputBase';
import * as React from 'react';
import { Button } from '@mui/material';

const Search = styled('div')(({ theme }) => ({
    position: 'relative',
    borderRadius: theme.shape.borderRadius,
    backgroundColor: alpha(theme.palette.common.white, 0.15),
    '&:hover': {
      backgroundColor: alpha(theme.palette.common.white, 0.25),
    },
    marginRight: theme.spacing(2),
    marginLeft: 0,
    width: '100%',
    [theme.breakpoints.up('sm')]: {
      marginLeft: theme.spacing(3),
      width: 'auto',
    },
}));

const SearchIconWrapper = styled('div')(({ theme }) => ({
    height: '100%',
    position: 'absolute',
    display: '-webkit-inline-box',
    alignItems: 'center',
    justifyContent: 'center',
}));

const StyledInputBase = styled(InputBase)(({ theme }) => ({
    color: 'inherit',
    '& .MuiInputBase-input': {
        padding: theme.spacing(1, 0, 1, 1),
        // vertical padding + font size from searchIcon
        paddingRight: `calc(1em + ${theme.spacing(4)})`,
        transition: theme.transitions.create('width'),
        width: '100%',
        [theme.breakpoints.up('md')]: {
            width: '20ch',
        },
    },
}));

export interface SearchBarProps {
    HandleChange: (event: React.ChangeEvent<HTMLInputElement>) => void,
    HandleSearch: (event: React.MouseEvent<HTMLButtonElement>) => void
}

export const SearchBar = (Props: SearchBarProps) => {
    return (
        <form>
            <Search>
                <StyledInputBase
                placeholder="Search…"
                inputProps={{ 'aria-label': 'search', onChange: Props.HandleChange }}
                />
                <Button type="submit" onClick={Props.HandleSearch} sx={{height: 39}}>
                    <SearchIconWrapper>
                        <SearchIcon />
                    </SearchIconWrapper>
                </Button>
            </Search>
        </form>
    );
}