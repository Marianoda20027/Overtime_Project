import { useState, useEffect } from 'react';

export const useRequests = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);

  // Función para obtener solicitudes desde el backend
  const fetchRequests = async () => {
    setLoading(true);
    try {
      // Endpoint real para obtener solicitudes de horas extra
      const token = localStorage.getItem("authToken");  // Asegúrate de tener el token JWT
      const response = await fetch('/api/overtime/all', {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,  // Incluye el token en el encabezado
        }
      });

      if (!response.ok) {
        throw new Error('Error al obtener solicitudes');
      }

      const data = await response.json();
      setRequests(data);  // Actualiza el estado con las solicitudes obtenidas del backend
    } catch (error) {
      console.error('Error al obtener solicitudes:', error);
      setRequests([]);  // Manejo de errores
    } finally {
      setLoading(false);  // Finaliza la carga
    }
  };

  // Cargar solicitudes al montar el componente
  useEffect(() => {
    fetchRequests();
  }, []);

  // Función para aceptar solicitud
  const acceptRequest = async (id, comments, cost) => {
    try {
      const token = localStorage.getItem("authToken");  // Obtén el token JWT
      const response = await fetch(`/api/overtime/${id}/accept`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify({
          comments: comments,
          cost: cost
        })
      });

      if (!response.ok) {
        throw new Error('Error al aceptar la solicitud');
      }

      const result = await response.json();
      console.log('Solicitud aceptada exitosamente:', result);
      
      // Recargar la lista de solicitudes después de aceptar
      fetchRequests();
      
    } catch (error) {
      console.error('Error al aceptar solicitud:', error);
    }
  };

  // Función para rechazar solicitud
  const rejectRequest = async (id, reason, comments, cost) => {
    try {
      const token = localStorage.getItem("authToken");  // Obtén el token JWT
      const response = await fetch(`/api/overtime/${id}/reject`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify({
          reason: reason,
          comments: comments,
          cost: cost
        })
      });

      if (!response.ok) {
        throw new Error('Error al rechazar la solicitud');
      }

      const result = await response.json();
      console.log('Solicitud rechazada exitosamente:', result);
      
      // Recargar la lista de solicitudes después de rechazar
      fetchRequests();
      
    } catch (error) {
      console.error('Error al rechazar solicitud:', error);
    }
  };

  return { 
    requests, 
    acceptRequest, 
    rejectRequest, 
    loading,
    refetch: fetchRequests // Método para recargar manualmente las solicitudes
  };
};
