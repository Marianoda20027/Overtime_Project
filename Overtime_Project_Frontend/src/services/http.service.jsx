// src/services/http.service.js
import axios from 'axios';
import Cookies from 'js-cookie';

const API_URL = "http://localhost:5100";

// Crear la instancia de Axios
const createApiInstance = (baseURL) => {
  const instance = axios.create({ baseURL });

  // Interceptor de request
  instance.interceptors.request.use(
    (config) => {
      const token = Cookies.get('jwt'); // Obtener el token de las cookies
      if (token) {
        // Si el token existe, lo agregamos a los headers de la solicitud
        config.headers['Authorization'] = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject({ status: 0, message: `Request failed: ${error.message}` });
    }
  );

  // Interceptor de response para manejar errores
  instance.interceptors.response.use(
    (response) => response,
    (error) => {
      const status = error.response?.status || 0;
      const code = error.response?.data?.code || '';
      const dataMessage = error.response?.data?.message || '';

      let friendlyMessage = 'An unknown error occurred';

      switch (status) {
        case 400:
          friendlyMessage = dataMessage || 'Invalid data or request';
          break;
        case 401:
          friendlyMessage = dataMessage || 'Unauthorized user';
          break;
        case 403:
          friendlyMessage = 'You do not have permission for this action';
          break;
        case 404:
          friendlyMessage = 'Resource not found';
          break;
        case 500:
          friendlyMessage = 'Server error, please try again later';
          break;
        default:
          friendlyMessage = dataMessage || error.message || 'Unknown error';
      }

      return Promise.reject({ status, message: friendlyMessage });
    }
  );

  return instance;
};

export const httpService = createApiInstance(API_URL);