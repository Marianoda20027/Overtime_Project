
import { useState, useEffect, useMemo, useCallback } from 'react';
import OvertimeService from '../../services/overtime.service';

export const useOvertimeForm = () => {
  const [form, setForm] = useState({
    date: '',
    startTime: '',
    endTime: '',
    justification: '',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [okMsg, setOkMsg] = useState(null);
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
    setError(null);
    setOkMsg(null);

     if (isInvalidTime) {
      setError('La hora final debe ser posterior a la hora de inicio');
      return;
    }

    if (totalHours <= 0) {
      setError('Las horas deben ser mayores que 0');
      return;
    }

    setLoading(true);

    try {
      
      const tokenData = await OvertimeService.getJwtData();
      const email = tokenData?.sub; 

      if (!email) {
        setError('Usuario no autenticado');
        return;
      }

      
      await OvertimeService.create({
        ...form,
        totalHours,
        email, 
      });
      setOkMsg('Solicitud enviada correctamente');
      setForm({ date: '', startTime: '', endTime: '', justification: '' });
    } catch (err) {
      setError(err.message || 'Error al enviar la solicitud');
    } finally {
      setLoading(false);
    }
  }, [form, totalHours]);

  
  useEffect(() => {
    async function fetchRequests() {
      const data = await OvertimeService.myRequests();
      setRequests(data);
    }
    fetchRequests();
  }, []);

  return { form, update, totalHours, loading, error, okMsg, submit, requests, isInvalidTime };
};
