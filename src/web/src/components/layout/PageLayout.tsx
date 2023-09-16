import Box from '@mui/material/Box';
import { ThemeProvider, createTheme } from '@mui/material';
import { NavigationBar } from '../navigation/NavigationBar';

export interface PageLayoutProps {
    children?: React.ReactNode
}

export const PageLayout = (Props: PageLayoutProps) => {
    /**
     * Most applications will need to conditionally render certain components based on whether a user is signed in or not.
     * msal-react provides 2 easy ways to do this. AuthenticatedTemplate and UnauthenticatedTemplate components will
     * only render their children if a user is authenticated or unauthenticated, respectively.
     */
    
    const defaultTheme = createTheme();

    return (
        <>
            <ThemeProvider theme={defaultTheme}>
                <NavigationBar />
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
                    {Props.children}
                </Box>
            </ThemeProvider>
        </>
    );
}