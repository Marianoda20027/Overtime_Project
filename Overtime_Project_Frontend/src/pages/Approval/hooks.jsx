import { useState, useEffect } from 'react';
import Cookies from 'js-cookie'; // solo esta sí, para leer la cookie
import { decodeJWT } from '../../hooks/decodeJWT.JSX';

export const useRequests = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const API_BASE = "http://localhost:5100";

  const fetchRequests = async () => {
    setLoading(true);
    try {
      const token = Cookies.get("jwt");
      if (!token) {
        console.warn("No hay token en las cookies.");
        setLoading(false);
        return;
      }

      // Decodificar el token manualmente
      const decoded = decodeJWT(token);
      const role = decoded?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      const email = decoded?.sub;
      console.log("Rol:", role, "| Email:", email);

      // ⚙️ Aquí podrías hacer fetch del managerId si no lo tenés guardado
      let url = `${API_BASE}/api/manager/${email}`;



      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) throw new Error("Error al obtener solicitudes");

      const data = await response.json();
      setRequests(data);
    } catch (error) {
      console.error("Error al obtener solicitudes:", error);
      setRequests([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  return { requests, loading, refetch: fetchRequests };
};
