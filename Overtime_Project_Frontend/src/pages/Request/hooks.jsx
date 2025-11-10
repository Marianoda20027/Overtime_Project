import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
import toast from 'react-hot-toast';
import { decodeJWT } from '../../hooks/decodeJWT.jsx';

export const useRequests = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);

  // 🚀 Cambiá esta URL al dominio público de Railway
  const API_BASE = "https://overtimeproject-production.up.railway.app";

  const fetchRequests = async () => {
    setLoading(true);

    try {
      const token = Cookies.get("jwt");
      if (!token) {
        toast.error("Session expired. Please log in again.", {
          icon: '🔒',
          duration: 5000,
        });
        throw new Error("No token found");
      }

      const decoded = decodeJWT(token);
      const email = decoded?.sub;
      
      if (!email) {
        toast.error("Could not retrieve user information. Please log in again.", {
          icon: '⚠️',
          duration: 5000,
        });
        throw new Error("No email found in token");
      }

      const response = await fetch(`${API_BASE}/api/overtime/user/${email}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        const errorMessage = errorData.message || "Failed to load requests";
        throw new Error(errorMessage);
      }

      const data = await response.json();
      setRequests(data);

    } catch (err) {
      console.error("Error fetching requests:", err);

      if (!err.message.includes("token") && !err.message.includes("email")) {
        if (err.message.includes('fetch')) {
          toast.error("Cannot connect to server. Please check your connection.", {
            icon: '🌐',
            duration: 5000,
          });
        }
      }

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
