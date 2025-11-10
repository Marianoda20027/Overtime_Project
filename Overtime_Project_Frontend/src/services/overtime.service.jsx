// src/services/overtime.service.js
import { httpService } from './http.service';
import Cookies from 'js-cookie';
import { decodeJWT } from '../hooks/decodeJWT.jsx';

class OvertimeService {
  base = 'api/overtime';

  // Crear solicitud de horas extra
  async create(payload) {
    const { data } = await httpService.post(`${this.base}/create`, payload);
    return data;
  }

  // Obtener las solicitudes de horas extra de un usuario
  async myRequests() {
    try {
      const tokenData = await this.getJwtData();
      const email = tokenData?.sub;
      
      if (!email) {
        throw new Error('User not authenticated');
      }

      // Usa el endpoint correcto con el email del usuario
      const { data } = await httpService.get(`${this.base}/user/${email}`);
      return data;
    } catch (error) {
      console.error('Error fetching my requests:', error);
      throw error;
    }
  }

  // Obtener solicitud de horas extra por ID
  async getById(id) {
    const { data } = await httpService.get(`${this.base}/${id}`);
    return data;
  }

  // Obtener datos del JWT
  async getJwtData() {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('No token found');
    }
    
    try {
      // Usa la función decodeJWT que ya tienes
      return decodeJWT(token);
    } catch (error) {
      console.error('Error decoding JWT:', error);
      throw new Error('Invalid token');
    }
  }
}

export default new OvertimeService();