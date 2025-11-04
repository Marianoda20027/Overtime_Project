import React from 'react';
import { useOvertimeForm } from './hooks.jsx';
import './styles.css';
import { useNavigate } from 'react-router-dom';

const OvertimeRequest = () => {
  const { form, update, totalHours, loading, submit, isInvalidTime } = useOvertimeForm();
  const navigate = useNavigate();

  const handleViewRequests = () => {
    navigate('/request');
  };

  return (
    <div className="ot-page">
      <div className="ot-card">
        <h3>Overtime Request</h3>

        <form onSubmit={submit}>
          <div className="input-group">
            <label>Date</label>
            <input
              type="date"
              value={form.date}
              onChange={(e) => update('date', e.target.value)}
              required
              disabled={loading}
            />
          </div>

          <div className="input-group">
            <label>Start Time</label>
            <input
              type="time"
              value={form.startTime}
              onChange={(e) => update('startTime', e.target.value)}
              required
              disabled={loading}
            />
          </div>

          <div className="input-group">
            <label>End Time</label>
            <input
              type="time"
              value={form.endTime}
              onChange={(e) => update('endTime', e.target.value)}
              required
              disabled={loading}
            />
          </div>

          {/* 💡 Mensaje en DOM - Feedback contextual inmediato */}
          {form.startTime && form.endTime && (
            <div className={`time-info ${isInvalidTime ? 'invalid' : 'valid'}`}>
              {isInvalidTime ? (
                <>
                  <span className="icon">⚠️</span>
                  <span>End time must be after start time</span>
                </>
              ) : (
                <>
                  <span className="icon">✓</span>
                  <span>Total hours: <strong>{totalHours}</strong></span>
                </>
              )}
            </div>
          )}

          <div className="input-group">
            <label>Justification</label>
            <textarea
              rows={4}
              value={form.justification}
              onChange={(e) => update('justification', e.target.value)}
              placeholder="Explain the reason for your overtime request..."
              required
              disabled={loading}
            />
          </div>

          <button className="ot-btn" type="submit" disabled={loading || isInvalidTime}>
            {loading ? (
              <>
                <span className="spinner"></span>
                Submitting...
              </>
            ) : (
              'Submit Request'
            )}
          </button>
        </form>

        <button className="ot-btn view-requests-btn" onClick={handleViewRequests}>
          View My Requests
        </button>
      </div>
    </div>
  );
};

export default OvertimeRequest;