export class ErrorHandler {
  // Método para mapear códigos de error a mensajes amigables
  static getErrorMessage(error) {
    // Si es un error de axios
    if (error.response) {
      switch (error.response.status) {
        case 400:
          return 'Datos inválidos. Por favor revisa los campos.';
        case 401:
          return 'Usuario o contraseña incorrectos.';
        case 403:
          return 'No tienes permisos para acceder a esta acción.';
        case 404:
          return 'Usuario no encontrado o endpoint incorrecto.';
        case 500:
          return 'Error del servidor. Intenta más tarde.';
        default:
          return `Error desconocido: ${error.response.status}`;
      }
    } else if (error.request) {
      // No hubo respuesta del servidor
      return 'No se pudo conectar al servidor. Revisa tu conexión.';
    } else {
      // Otros errores
      return error.message || 'Ocurrió un error desconocido';
    }
  }
}
