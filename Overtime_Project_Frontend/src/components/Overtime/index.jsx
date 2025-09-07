// src/pages/Overtime/index.jsx
import React from 'react';
import { useOvertimeForm } from './hooks.jsx';
import './styles.css'; // Importing the updated styles
import { useNavigate } from 'react-router-dom';

const OvertimeRequest = () => {
  const { form, update, totalHours, loading, error, okMsg, submit } = useOvertimeForm();

  const navigate = useNavigate(); // Aquí es donde obtenemos la función navigate


  // Función para manejar la acción del botón "Ver Solicitudes"
  const handleViewRequests = () => {
      navigate('/request'); // Navegamos a la página OvertimeRequest

  };

  return (
    <div className="ot-page">
      <div className="ot-card">
        <h3>Overtime Request</h3>

        {/* Overtime request form */}
        <form onSubmit={submit}>
          <div className="input-group">
            <label>Date</label>
            <input
              type="date"
              value={form.date}
              onChange={(e) => update('date', e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>Start Time</label>
            <input
              type="time"
              value={form.startTime}
              onChange={(e) => update('startTime', e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>End Time</label>
            <input
              type="time"
              value={form.endTime}
              onChange={(e) => update('endTime', e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>Cost Center (optional)</label>
            <input
              type="text"
              value={form.costCenter || ''}
              onChange={(e) => update('costCenter', e.target.value)}
            />
          </div>

          <div className="input-group">
            <label>Justification</label>
            <textarea
              rows={4}
              value={form.justification}
              onChange={(e) => update('justification', e.target.value)}
              required
            />
          </div>

          {/* Botón para enviar la solicitud */}
          <button className="ot-btn" type="submit" disabled={loading}>
            {loading ? 'Sending...' : 'Submit Request'}
          </button>
        </form>

        {/* Botón para ver solicitudes anteriores */}
        <button className="ot-btn view-requests-btn" onClick={handleViewRequests}>
          View My Requests
        </button>

        {error && <div className="error-message">{error}</div>}
        {okMsg && <div className="success-message">{okMsg}</div>}
      </div>
    </div>
  );
};

export default OvertimeRequest;
