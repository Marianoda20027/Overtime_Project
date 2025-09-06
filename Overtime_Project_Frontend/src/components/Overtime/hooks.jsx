// src/pages/Overtime/hooks.js
import { useState, useEffect, useMemo, useCallback } from 'react';
import OvertimeService from '../../services/overtime.service';

export const useOvertimeForm = () => {
  const [form, setForm] = useState({
    date: '',
    startTime: '',
    endTime: '',
    justification: '',
    costCenter: '',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [okMsg, setOkMsg] = useState(null);
  const [requests, setRequests] = useState([]);

  // Calcular las horas totales
  const totalHours = useMemo(() => {
    if (!form.startTime || !form.endTime) return 0;
    const [sh, sm] = form.startTime.split(':').map(Number);
    const [eh, em] = form.endTime.split(':').map(Number);
    const start = sh + sm / 60;
    const end = eh + em / 60;
    const diff = end - start;
    return diff > 0 ? diff.toFixed(2) : 0;
  }, [form.startTime, form.endTime]);

  // Manejar cambios en el formulario
  const update = useCallback((key, value) => {
    setForm((prevForm) => ({ ...prevForm, [key]: value }));
  }, []);

  // Enviar la solicitud de horas extra
  const submit = useCallback(async (e) => {
    e.preventDefault();
    setError(null);
    setOkMsg(null);

    if (totalHours <= 0) {
      setError('Las horas deben ser mayores que 0');
      return;
    }

    setLoading(true);

    try {
      await OvertimeService.create({
        ...form,
        totalHours,
      });
      setOkMsg('Solicitud enviada correctamente');
      setForm({ date: '', startTime: '', endTime: '', justification: '', costCenter: '' });
    } catch (err) {
      setError(err.message || 'Error al enviar la solicitud');
    } finally {
      setLoading(false);
    }
  }, [form, totalHours]);

  // Obtener solicitudes de horas extra al cargar el componente
  useEffect(() => {
    async function fetchRequests() {
      const data = await OvertimeService.myRequests();
      setRequests(data);
    }
    fetchRequests();
  }, []);

  return { form, update, totalHours, loading, error, okMsg, submit, requests };
};
