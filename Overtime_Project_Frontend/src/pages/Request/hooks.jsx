import { useState, useEffect } from 'react';

export const useYourRequests = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Función para obtener requests desde la API
  const fetchRequests = async () => {
    try {
      setLoading(true);
      setError(null);

      // Simular delay de API
      await new Promise(resolve => setTimeout(resolve, 1500));

      // Datos de ejemplo para testing
      const mockData = [
        {
          id: 1,
          date: '2024-09-05',
          startTime: '18:00',
          endTime: '22:00',
          totalHours: 4,
          status: 'approved',
          justification: 'System maintenance and security updates'
        },
        {
          id: 2,
          date: '2024-09-03',
          startTime: '17:30',
          endTime: '20:30',
          totalHours: 3,
          status: 'pending',
          justification: 'Emergency bug fix for production issue'
        },
        {
          id: 3,
          date: '2024-09-01',
          startTime: '19:00',
          endTime: '23:00',
          totalHours: 4,
          status: 'rejected',
          justification: 'Database migration during off-peak hours'
        }
      ];

      // Simula un pequeño retraso extra para hacer la carga más suave
      await new Promise(resolve => setTimeout(resolve, 500));

      setRequests(mockData);
    } catch (err) {
      setError('Error loading requests');
    } finally {
      setLoading(false);
    }
  };

  // Función para eliminar un request
  const deleteRequest = async (id) => {
    try {
      // Simular delay de API
      await new Promise(resolve => setTimeout(resolve, 500));
      
      setRequests(prev => prev.filter(req => req.id !== id));
    } catch (err) {
      setError('Error deleting request');
    }
  };

  // Cargar requests al inicio
  useEffect(() => {
    fetchRequests();
  }, []);

  // Estadísticas simples
  const totalRequests = requests.length;
  const totalHours = requests.reduce((sum, req) => sum + (req.totalHours || 0), 0);
  const pendingRequests = requests.filter(req => req.status === 'pending').length;
  const approvedRequests = requests.filter(req => req.status === 'approved').length;

  return {
    requests,
    loading,
    error,
    refreshRequests: fetchRequests,
    deleteRequest,
    totalRequests,
    totalHours,
    pendingRequests,
    approvedRequests
  };
};
