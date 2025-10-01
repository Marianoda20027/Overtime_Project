// src/services/overtime.service.js
import { httpService } from './http.service';
import Cookies from 'js-cookie';

class OvertimeService {
  base = 'api/overtime';

  // Crear solicitud de horas extra
  async create(payload) {
    const { data } = await httpService.post(`${this.base}/create`, payload);
    return data;
  }

  // Obtener las solicitudes de horas extra de un usuario
  async myRequests() {
    const { data } = await httpService.get(`${this.base}/me`);
    return data;
  }

  // Obtener solicitud de horas extra por ID
  async getById(id) {
    const { data } = await httpService.get(`${this.base}/${id}`);
    return data;
  }

  // Obtener datos del JWT (opcional si el backend lo necesita)
  async getJwtData() {
    const token = Cookies.get('jwt');
    if (token) {
      return JSON.parse(atob(token.split('.')[1])); // Decodificando el JWT
    }
    return null;
  }
}

export default new OvertimeService();
