import axios from 'axios';
import router from '../router';

export const httpClient = axios.create({
    baseURL: 'https://api.esok24.pl',
    headers: {
        Authorization: 'Bearer ' + localStorage.getItem('auth_token'),
        'Content-Type': 'application/json'
    }
})

httpClient.interceptors.response.use((response) => response, (error) => {
    if (error.response.status === 401) {
        router.push(
            { name: "login" }
        );
    }
    throw error;
});