import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
import { decodeJWT } from '../../hooks/decodeJWT.JSX';

export const useRequests = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const API_BASE = "http://localhost:5100";

  const fetchRequests = async () => {
    setLoading(true);
    setError(null);

    try {
      const token = Cookies.get("jwt");
      if (!token) throw new Error("No se encontró el token.");

      const decoded = decodeJWT(token);
      const email = decoded?.sub;
      if (!email) throw new Error("No se pudo obtener el email del usuario.");

      const response = await fetch(`${API_BASE}/api/overtime/user/${email}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(text || "Error al obtener solicitudes.");
      }

      const data = await response.json();
      setRequests(data);

    } catch (err) {
      console.error("Error al obtener solicitudes:", err);
      setError(err.message || "Error desconocido al cargar solicitudes.");
      setRequests([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  return { requests, loading, error, refetch: fetchRequests };
};