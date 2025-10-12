import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
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

      const decoded = decodeJWT(token);
      const email = decoded?.sub;

      if (!email) {
        console.warn("No se pudo obtener el email del token");
        setLoading(false);
        return;
      }

      const response = await fetch(`${API_BASE}/api/manager/${email}`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) throw new Error("Error al obtener solicitudes");

      const data = await response.json();
      
      // 🔥 FILTRAR solo Pending
      const pendingRequests = data.filter(req => req.status === "Pending");
      setRequests(pendingRequests);
      
    } catch (error) {
      console.error("Error al obtener solicitudes:", error);
      setRequests([]);
    } finally {
      setLoading(false);
    }
  };

  // ✅ APROBAR SOLICITUD
  const acceptRequest = async (overtimeId, comments) => {
    try {
      const token = Cookies.get("jwt");
      if (!token) throw new Error("No token found");

      const decoded = decodeJWT(token);
      const managerEmail = decoded?.sub;

      if (!managerEmail) throw new Error("No se pudo obtener el email del manager");

      const response = await fetch(`${API_BASE}/api/overtime/approve/${overtimeId}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          managerEmail,
          comments: comments || null
        })
      });

      if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || "Error al aprobar");
      }

      // Refrescar la lista
      await fetchRequests();
      return true;

    } catch (error) {
      console.error("Error al aprobar solicitud:", error);
      throw error;
    }
  };

  // ❌ RECHAZAR SOLICITUD
  const rejectRequest = async (overtimeId, reason, comments) => {
    try {
      const token = Cookies.get("jwt");
      if (!token) throw new Error("No token found");

      const decoded = decodeJWT(token);
      const managerEmail = decoded?.sub;

      if (!managerEmail) throw new Error("No se pudo obtener el email del manager");

      const response = await fetch(`${API_BASE}/api/overtime/reject/${overtimeId}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          managerEmail,
          reason,
          comments: comments || null
        })
      });

      if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || "Error al rechazar");
      }

      // Refrescar la lista
      await fetchRequests();
      return true;

    } catch (error) {
      console.error("Error al rechazar solicitud:", error);
      throw error;
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  return { 
    requests, 
    loading, 
    acceptRequest, 
    rejectRequest, 
    refetch: fetchRequests 
  };
};