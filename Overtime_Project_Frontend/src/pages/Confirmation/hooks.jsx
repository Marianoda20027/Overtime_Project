import { useState, useEffect } from 'react';

export const useRequests = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);

  // Función para obtener solicitudes
  const fetchRequests = async () => {
    setLoading(true);
    try {
      // Aquí puedes cambiar por tu endpoint real
      // const response = await fetch('/api/requests');
      // const data = await response.json();
      
      // Datos simulados por ahora
      const data = [
        { 
          id: 1, 
          person: 'Juan Pérez', 
          date: '2025-09-15', 
          startTime: '09:00', 
          endTime: '13:00', 
          justification: 'Necesito completar el informe mensual que se entrega mañana. Es urgente porque el cliente lo requiere para su junta directiva.', 
          cost: 45000 
        },
        { 
          id: 2, 
          person: 'Ana López', 
          date: '2025-09-16', 
          startTime: '10:00', 
          endTime: '12:00', 
          justification: 'Reunión de equipo para revisar el proyecto del trimestre.', 
          cost: 32000 
        },
        { 
          id: 3, 
          person: 'Carlos Rodríguez', 
          date: '2025-09-17', 
          startTime: '14:00', 
          endTime: '18:00', 
          justification: 'Mantenimiento del servidor principal que no se puede hacer en horario normal.', 
          cost: 58000 
        },
        { 
          id: 4, 
          person: 'María González', 
          date: '2025-09-18', 
          startTime: '08:00', 
          endTime: '11:00', 
          justification: 'Capacitación del nuevo personal que ingresa la próxima semana.', 
          cost: 28000 
        },
        { 
          id: 5, 
          person: 'Pedro Martínez', 
          date: '2025-09-19', 
          startTime: '16:00', 
          endTime: '20:00', 
          justification: 'Implementación de nuevas funcionalidades en el sistema de producción.', 
          cost: 62000 
        },
        { 
          id: 6, 
          person: 'Laura Fernández', 
          date: '2025-09-20', 
          startTime: '07:00', 
          endTime: '10:00', 
          justification: 'Auditoría mensual que requiere acceso temprano a los sistemas.', 
          cost: 35000 
        }
      ];
      
      setRequests(data);
    } catch (error) {
      console.error('Error al obtener solicitudes:', error);
      setRequests([]);
    } finally {
      setLoading(false);
    }
  };

  // Cargar solicitudes al montar el componente
  useEffect(() => {
    fetchRequests();
  }, []);

  // Función para aceptar solicitud
  const acceptRequest = async (id) => {
    try {
      // POST request para aceptar
      const response = await fetch('/api/requests/accept', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          requestId: id,
          action: 'accept',
          timestamp: new Date().toISOString()
        })
      });

      if (!response.ok) {
        throw new Error('Error al aceptar la solicitud');
      }

      const result = await response.json();
      console.log('Solicitud aceptada exitosamente:', result);
      
      // Recargar la página completa
      window.location.reload();
      
    } catch (error) {
      console.error('Error al aceptar solicitud:', error);
      
      // Simulación: remover de la lista local y recargar
      console.log('Solicitud Aceptada (simulación):', id);
      setTimeout(() => {
        window.location.reload();
      }, 500);
    }
  };

  // Función para rechazar solicitud
  const rejectRequest = async (id, reason) => {
    try {
      // POST request para rechazar con razón
      const response = await fetch('/api/requests/reject', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          requestId: id,
          action: 'reject',
          reason: reason,
          timestamp: new Date().toISOString()
        })
      });

      if (!response.ok) {
        throw new Error('Error al rechazar la solicitud');
      }

      const result = await response.json();
      console.log('Solicitud rechazada exitosamente:', result);
      
      // Recargar la página completa
      window.location.reload();
      
    } catch (error) {
      console.error('Error al rechazar solicitud:', error);
      
      // Simulación: log y recargar
      console.log('Solicitud Rechazada (simulación):', id, 'Razón:', reason);
      setTimeout(() => {
        window.location.reload();
      }, 500);
    }
  };

  return { 
    requests, 
    acceptRequest, 
    rejectRequest, 
    loading,
    refetch: fetchRequests // Por si necesitas recargar manualmente
  };
};