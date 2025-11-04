import { useState, useEffect, useMemo, useCallback } from 'react';
import toast from 'react-hot-toast';
import OvertimeService from '../../services/overtime.service';

export const useOvertimeForm = () => {
  const [form, setForm] = useState({
    date: '',
    startTime: '',
    endTime: '',
    justification: '',
  });
  const [loading, setLoading] = useState(false);
  const [requests, setRequests] = useState([]);

  const totalHours = useMemo(() => {
    if (!form.startTime || !form.endTime) return 0;
    const [sh, sm] = form.startTime.split(':').map(Number);
    const [eh, em] = form.endTime.split(':').map(Number);
    const start = sh + sm / 60;
    const end = eh + em / 60;
    const diff = end - start;
    return diff > 0 ? diff.toFixed(2) : 0;
  }, [form.startTime, form.endTime]);

  const isInvalidTime = useMemo(() => {
    if (!form.startTime || !form.endTime) return false;
    const [sh, sm] = form.startTime.split(':').map(Number);
    const [eh, em] = form.endTime.split(':').map(Number);
    const start = sh + sm / 60;
    const end = eh + em / 60;
    return end <= start;
  }, [form.startTime, form.endTime]);

  const update = useCallback((key, value) => {
    setForm((prevForm) => ({ ...prevForm, [key]: value }));
  }, []);

  const submit = useCallback(async (e) => {
    e.preventDefault();

    // ⚠️ Validaciones - Mensajes en DOM (más sutiles)
    if (isInvalidTime) {
      toast.error('End time must be after start time', {
        icon: '⏰',
        duration: 4000,
      });
      return;
    }

    if (totalHours <= 0) {
      toast.error('Hours must be greater than 0', {
        icon: '⏰',
        duration: 4000,
      });
      return;
    }

    setLoading(true);

    try {
      const tokenData = await OvertimeService.getJwtData();
      const email = tokenData?.sub;

      if (!email) {
        toast.error('Session expired. Please log in again.', {
          icon: '🔒',
          duration: 5000,
        });
        setLoading(false);
        return;
      }

      await OvertimeService.create({
        ...form,
        totalHours,
        email,
      });

      // ✅ Toast de éxito - Acción importante completada
      toast.success('Overtime request submitted successfully!', {
        icon: '✅',
        duration: 3000,
      });

      // Limpiar formulario
      setForm({ date: '', startTime: '', endTime: '', justification: '' });

      // Refrescar lista de solicitudes
      const data = await OvertimeService.myRequests();
      setRequests(data);

    } catch (err) {
      // ❌ Toast de error - Algo salió mal
      const errorMessage = err.message || 'Failed to submit request. Please try again.';
      toast.error(errorMessage, {
        icon: '❌',
        duration: 5000,
      });
    } finally {
      setLoading(false);
    }
  }, [form, totalHours, isInvalidTime]);

  useEffect(() => {
    async function fetchRequests() {
      try {
        const data = await OvertimeService.myRequests();
        setRequests(data);
      } catch (err) {
        console.error('Error fetching requests:', err);
        // No mostramos toast aquí porque es un error silencioso al cargar
      }
    }
    fetchRequests();
  }, []);

  return { form, update, totalHours, loading, submit, requests, isInvalidTime };
};