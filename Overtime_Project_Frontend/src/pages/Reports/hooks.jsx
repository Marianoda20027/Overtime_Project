// src/pages/Reports/hooks.js
import { useState, useCallback } from 'react';
import toast from 'react-hot-toast';

export const useReport = () => {
  const [pdfUrl, setPdfUrl] = useState(null);
  const [loading, setLoading] = useState(false);
  const [generatingPdf, setGeneratingPdf] = useState(false);
  const [sendingEmail, setSendingEmail] = useState(false);

  const generateAndSendReport = useCallback(async (email) => {
    // Validación del email
    if (!email) {
      toast.error("Please provide a valid email address", {
        icon: '📧',
        duration: 4000,
      });
      return;
    }

    // Validación simple de formato de email
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      toast.error("Please enter a valid email format", {
        icon: '⚠️',
        duration: 4000,
      });
      return;
    }

    setLoading(true);
    setGeneratingPdf(true);

    try {
      // 🧾 1️⃣ Generate PDF
      const pdfResponse = await fetch('http://localhost:5100/api/reports/generate', {
        method: 'GET',
      });

      if (!pdfResponse.ok) {
        const errorData = await pdfResponse.json();
        throw new Error(errorData.message || 'Failed to generate report');
      }

      // Liberar URL anterior si existe
      if (pdfUrl) URL.revokeObjectURL(pdfUrl);

      const blob = await pdfResponse.blob();
      const url = URL.createObjectURL(blob);
      setPdfUrl(url);

      setGeneratingPdf(false);
      setSendingEmail(true);

      // 📧 2️⃣ Send Email
      const emailResponse = await fetch('http://localhost:5100/api/reports/send', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email }),
      });

      if (!emailResponse.ok) {
        const errorData = await emailResponse.json();
        throw new Error(errorData.message || 'Failed to send email');
      }

      // ✅ Success toast - Acción completada
      toast.success(`Report sent successfully to ${email}!`, {
        icon: '✉️',
        duration: 4000,
      });

    } catch (err) {
      console.error("Error generating or sending report:", err);
      
      // ❌ Error toast con mensaje específico
      let errorMessage = "An error occurred. Please try again.";
      
      if (err.message.includes('generate')) {
        errorMessage = "Could not generate the report. There may be no data available.";
      } else if (err.message.includes('send') || err.message.includes('email')) {
        errorMessage = "Report generated but failed to send email. Please check the email address.";
      } else if (err.message.includes('fetch')) {
        errorMessage = "Cannot connect to server. Please check your connection.";
      }

      toast.error(errorMessage, {
        icon: '❌',
        duration: 5000,
      });

    } finally {
      setLoading(false);
      setGeneratingPdf(false);
      setSendingEmail(false);
    }
  }, [pdfUrl]);

  return { 
    pdfUrl, 
    loading, 
    generatingPdf, 
    sendingEmail, 
    generateAndSendReport 
  };
};