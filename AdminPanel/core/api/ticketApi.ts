import axios from 'axios';

// Instancia de Axios configurada para apuntar al backend local
export const ticketApi = axios.create({
    baseURL: 'http://localhost:5077/api' 
});
