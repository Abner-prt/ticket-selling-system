import axios from 'axios';

// Instancia de Axios configurada para apuntar al backend local
export const ticketApi = axios.create({
    baseURL: 'http://localhost:5077/api' 
});

// Interceptor para agregar el token JWT a todas las peticiones protegidas
ticketApi.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);
