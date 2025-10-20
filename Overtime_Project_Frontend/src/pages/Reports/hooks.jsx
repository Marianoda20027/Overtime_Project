// src/pages/Reports/hooks.js
import { useState, useCallback } from 'react';
import { httpService } from '../../services/http.service';

export const useReport = () => {
  const [pdfUrl, setPdfUrl] = useState(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState(null);
  const [error, setError] = useState(null);

  const generateAndSendReport = useCallback(async (email) => {
    setLoading(true);
    setError(null);
    setMessage(null);

    try {
      // 1️⃣ Generar el PDF y mostrarlo
      const res = await httpService.get('/reports/generate', {
        responseType: 'blob',
      });
      const blob = new Blob([res.data], { type: 'application/pdf' });
      const url = URL.createObjectURL(blob);
      setPdfUrl(url);

      // 2️⃣ Enviar el reporte al correo
      await httpService.post('/reports/send', JSON.stringify(email), {
        headers: { 'Content-Type': 'application/json' },
      });

      setMessage(`✅ Reporte generado y enviado a ${email}`);
    } catch (err) {
      console.error(err);
      setError('❌ Error generando o enviando el reporte.');
    } finally {
      setLoading(false);
    }
  }, []);

  return { pdfUrl, loading, message, error, generateAndSendReport };
};
