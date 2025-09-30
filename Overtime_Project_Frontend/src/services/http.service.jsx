// src/services/http.service.js
import axios from 'axios';

const API_URL = "http://localhost:5100";

const createApiInstance = (baseURL) => {
  const instance = axios.create({ baseURL });

  // Interceptor de request
  instance.interceptors.request.use(
    (config) => {
      // Aquí podés agregar token si lo tenés guardado
      // const token = AuthStorage.getToken();
      // if (token) config.headers['Authorization'] = `Bearer ${token}`;
      return config;
    },
    (error) => {
      // Error antes de enviar la petición
      return Promise.reject({ status: 0, message: `Request failed: ${error.message}` });
    }
  );

  // Interceptor de response
  instance.interceptors.response.use(
    (response) => response,
    (error) => {
      const status = error.response?.status || 0;
      const code = error.response?.data?.code || '';
      const dataMessage = error.response?.data?.message || '';

      let friendlyMessage = 'Ocurrió un error desconocido';

      switch (status) {
        case 400:
          friendlyMessage = dataMessage || 'Datos incorrectos o solicitud inválida';
          break;
        case 401:
          friendlyMessage = dataMessage || 'Usuario no autorizado';
          break;
        case 403:
          friendlyMessage = 'No tienes permisos para esta acción';
          break;
        case 404:
          friendlyMessage = 'Recurso no encontrado';
          break;
        case 500:
          friendlyMessage = 'Error del servidor, intenta nuevamente más tarde';
          break;
        default:
          friendlyMessage = dataMessage || error.message || 'Error desconocido';
      }

      return Promise.reject({ status, message: friendlyMessage });
    }
  );

  return instance;
};

export const httpService = createApiInstance(API_URL);
