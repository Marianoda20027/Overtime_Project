// src/utilities/errorLoginHandler.js
export class ErrorHandler {
  static getErrorMessage(error) {
    // Si es un error de axios con respuesta del servidor
    if (error.response) {
      const status = error.response.status;
      const serverMessage = error.response.data?.message;

      switch (status) {
        case 400:
          return serverMessage || 'Invalid data. Please check your information and try again.';
        case 401:
          return 'Incorrect email or password. Please try again.';
        case 403:
          return 'Access denied. You do not have permission to perform this action.';
        case 404:
          return 'Account not found. Please verify your email address.';
        case 500:
          return 'Server error. Our team has been notified. Please try again later.';
        default:
          return serverMessage || `Unexpected error occurred (Error ${status}). Please try again.`;
      }
    } 
    
    // Si no hay respuesta del servidor (problemas de red)
    else if (error.request) {
      return 'Unable to connect to the server. Please check your internet connection and try again.';
    } 
    
    // Otros errores
    else {
      return error.message || 'An unexpected error occurred. Please try again.';
    }
  }

  // Método adicional para traducir errores técnicos del backend
  static translateBackendError(technicalMessage) {
    const translations = {
      'Network Error': 'Connection problem. Please check your internet.',
      'timeout': 'Request took too long. Please try again.',
      'ECONNREFUSED': 'Cannot reach the server. Please try again later.',
      'invalid token': 'Your session has expired. Please log in again.',
      'jwt expired': 'Your session has expired. Please log in again.',
      'unauthorized': 'Authentication required. Please log in.',
    };

    for (const [key, value] of Object.entries(translations)) {
      if (technicalMessage.toLowerCase().includes(key.toLowerCase())) {
        return value;
      }
    }

    return technicalMessage;
  }
}