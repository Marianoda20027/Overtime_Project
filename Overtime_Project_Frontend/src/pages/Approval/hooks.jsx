import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
import { decodeJWT } from '../../hooks/decodeJWT.jsx';

export const useRequests = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);

  // 🔥 Cambiá el base URL al dominio de Railway
  const API_BASE = "https://overtimeproject-production.up.railway.app";

  const fetchRequests = async () => {
    setLoading(true);
    try {
      const token = Cookies.get("jwt");
      if (!token) {
        console.warn("No token found in cookies.");
        setLoading(false);
        return;
      }

      const decoded = decodeJWT(token);
      const email = decoded?.sub;

      if (!email) {
        console.warn("Could not retrieve email from token");
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

      if (!response.ok) throw new Error("Error fetching requests");

      const data = await response.json();
      const pendingRequests = data.filter(req => req.status === "Pending");
      setRequests(pendingRequests);
      
    } catch (error) {
      console.error("Error fetching requests:", error);
      setRequests([]);
    } finally {
      setLoading(false);
    }
  };

  const acceptRequest = async (overtimeId, comments) => {
    try {
      const token = Cookies.get("jwt");
      if (!token) throw new Error("No token found");

      const decoded = decodeJWT(token);
      const managerEmail = decoded?.sub;

      if (!managerEmail) throw new Error("Could not retrieve manager email");

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
        throw new Error(error.message || "Error approving request");
      }

      await fetchRequests(); // refrescar lista
      return true;

    } catch (error) {
      console.error("Error approving request:", error);
      throw error;
    }
  };

  const rejectRequest = async (overtimeId, reason, comments) => {
    try {
      const token = Cookies.get("jwt");
      if (!token) throw new Error("No token found");

      const decoded = decodeJWT(token);
      const managerEmail = decoded?.sub;

      if (!managerEmail) throw new Error("Could not retrieve manager email");

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
        throw new Error(error.message || "Error rejecting request");
      }

      await fetchRequests(); // refrescar lista
      return true;

    } catch (error) {
      console.error("Error rejecting request:", error);
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
