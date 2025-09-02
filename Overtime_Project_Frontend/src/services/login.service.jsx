// loginService.js
import { httpService } from './httpService';

// Login con usuario y contraseña
export const login = async (username, password) => {
  try {
    const payload = { username, password };
    const response = await httpService.post('/auth/login', payload);  // Realizamos el POST a /auth/login
    return response.data;  // Retornamos los datos del servidor (puede incluir token)
  } catch (error) {
    throw new Error(`Login failed: ${error.message || 'Unknown error'}`);  // Si falla, lanzamos un error
  }
};

// Login con SSO (Redirige a un flujo de autenticación SSO)
export const loginWithSSO = async () => {
  try {
    // Redirige a un servicio de SSO (esto es solo un ejemplo, necesitarías configurar tu proveedor de SSO)
    window.location.href = `${import.meta.env.VITE_API_URL}/auth/sso`;  // Aquí iría la URL de redirección del SSO
  } catch (error) {
    throw new Error(`SSO Login failed: ${error.message || 'Unknown error'}`);
  }
};
