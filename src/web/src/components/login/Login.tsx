
import { Button } from '@mui/material';
import { useMsal } from '@azure/msal-react';
import { loginRequest } from '../../authConfig';

export const Login = () => {
    const { instance } = useMsal();

    const handleLogin = () => {
        instance
            .loginPopup({
                ...loginRequest,
                redirectUri: '/'
            })
            .catch((error) => console.log(error));
    }

    return (
        <Button onClick={handleLogin}>
            Login
        </Button>
    )
}