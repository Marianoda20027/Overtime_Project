// src/pages/Reports/hooks.js
import { useState, useCallback } from 'react';

export const useReport = () => {
  const [pdfUrl, setPdfUrl] = useState(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState(null);
  const [error, setError] = useState(null);

  const generateAndSendReport = useCallback(async (email) => {
    if (!email) {
      setError("You must provide a valid email.");
      return;
    }

    setLoading(true);
    setError(null);
    setMessage(null);

    try {
      // 🧾 1️⃣ Generate PDF
      const pdfResponse = await fetch('http://localhost:5100/api/reports/generate', {
        method: 'GET',
      });

      if (!pdfResponse.ok) {
        throw new Error('Error generating PDF');
      }

      // Release previous URL if exists
      if (pdfUrl) URL.revokeObjectURL(pdfUrl);

      const blob = await pdfResponse.blob();
      const url = URL.createObjectURL(blob);
      setPdfUrl(url);

      // ✉️ 2️⃣ Send report via email (EXACTLY like your curl)
      const emailResponse = await fetch('http://localhost:5100/api/reports/send', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email }),
      });

      if (!emailResponse.ok) {
        throw new Error('Error sending email');
      }

      setMessage(`✅ Report generated and sent successfully to ${email}`);
    } catch (err) {
      console.error("Error generating or sending report:", err);
      setError("❌ Error generating or sending the report. Please try again.");
    } finally {
      setLoading(false);
    }
  }, [pdfUrl]);

  return { pdfUrl, loading, message, error, generateAndSendReport };
};