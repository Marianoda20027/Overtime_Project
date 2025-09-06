// src/services/overtime.service.js
import { httpService } from './http.service';

class OvertimeService {
  base = '/api/overtime-requests';

  // Crear solicitud de horas extra
  async create(payload) {
    const { data } = await httpService.post(this.base, payload);
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
}

export default new OvertimeService();
