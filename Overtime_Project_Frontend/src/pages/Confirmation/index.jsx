// src/pages/ConfirmationPage/index.jsx

import React, { useState } from 'react';
import { useRequests } from './hooks';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import './styles.css';

const ConfirmationPage = () => {
  const { requests, acceptRequest, rejectRequest, loading } = useRequests();
  const [showModal, setShowModal] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState(null);
  const [actionType, setActionType] = useState(''); // 'accept' o 'reject'
  const [reason, setReason] = useState('');
  const [comments, setComments] = useState(''); // Comentarios de la aprobación/rechazo
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleAccept = (request) => {
    setSelectedRequest(request);
    setActionType('accept');
    setShowModal(true);
  };

  const handleReject = (request) => {
    setSelectedRequest(request);
    setActionType('reject');
    setShowModal(true);
  };

  const handleClose = () => {
    setShowModal(false);
    setSelectedRequest(null);
    setActionType('');
    setReason('');
    setComments('');
    setIsSubmitting(false);
  };

  const handleConfirm = async () => {
    if (!selectedRequest) return;

    setIsSubmitting(true);

    try {
      // Calcular el costo de horas extra
      const cost = selectedRequest.totalHours * selectedRequest.salary; // salario por hora * total horas

      if (actionType === 'accept') {
        await acceptRequest(selectedRequest.id, comments, cost); // Aceptar solicitud con comentarios y costo
      } else if (actionType === 'reject') {
        if (!reason.trim()) {
          alert('Por favor, ingrese una razón para el rechazo');
          setIsSubmitting(false);
          return;
        }
        await rejectRequest(selectedRequest.id, reason.trim(), comments, cost); // Rechazar solicitud con razón, comentarios y costo
      }

      handleClose();

    } catch (error) {
      console.error('Error al procesar la solicitud:', error);
      alert('Error al procesar la solicitud. Intente nuevamente.');
      setIsSubmitting(false);
    }
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('es-ES', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const formatTime = (timeString) => {
    return new Date(`2000-01-01T${timeString}`).toLocaleTimeString('es-ES', {
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('es-CR', {
      style: 'currency',
      currency: 'CRC'
    }).format(amount);
  };

  // Loading state mejorado
  if (loading) {
    return (
      <div className="request-page">
        <Header />
        
        <h1 className="page-title">Solicitudes Pendientes</h1>
        
        <div className="loading-container">
          <div className="loading-spinner"></div>
          <p className="loading-text">
            Cargando solicitudes...
          </p>
        </div>
        
        <Footer />
      </div>
    );
  }

  return (
    <div className="request-page">
      <Header />
      
      <h1 className="page-title">Solicitudes Pendientes</h1>
      
      {requests.length === 0 ? (
        <div className="empty-state">
          <div className="empty-icon">📋</div>
          <h2 className="empty-title">No hay solicitudes pendientes</h2>
          <p className="empty-message">
            Cuando lleguen nuevas solicitudes de permisos, aparecerán aquí para su revisión.
          </p>
        </div>
      ) : (
        <div className="request-list">
          {requests.map((request) => (
            <div key={request.id} className="request-card">
              <div className="request-header">
                <h3 className="request-person">{request.person}</h3>
                <p className="request-date">{formatDate(request.date)}</p>
              </div>
              
              <div className="request-body">
                <div className="request-detail">
                  <span className="detail-label">Horario</span>
                  <div className="time-range">
                    <span>{formatTime(request.startTime)}</span>
                    <span>→</span>
                    <span>{formatTime(request.endTime)}</span>
                  </div>
                </div>
                
                <div className="justification">
                  <strong>Justificación:</strong> {request.justification}
                </div>
                
                <div className="request-detail">
                  <span className="detail-label">Costo estimado</span>
                  <span className="cost-highlight">
                    {formatCurrency(request.cost)}
                  </span>
                </div>
              </div>
              
              <div className="action-buttons">
                <button 
                  className="btn btn-accept"
                  onClick={() => handleAccept(request)}
                  disabled={isSubmitting}
                >
                  ✓ Aceptar
                </button>
                <button 
                  className="btn btn-reject"
                  onClick={() => handleReject(request)}
                  disabled={isSubmitting}
                >
                  ✗ Rechazar
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {showModal && selectedRequest && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3 className="modal-title">
                {actionType === 'accept' ? 'Aceptar Solicitud' : 'Rechazar Solicitud'}
              </h3>
            </div>
            
            <div className="modal-body">
              <p className="modal-text">
                {actionType === 'accept' 
                  ? `¿Está seguro que desea aceptar la solicitud de ${selectedRequest.person}?`
                  : `¿Está seguro que desea rechazar la solicitud de ${selectedRequest.person}?`
                }
              </p>
              
              <div style={{
                background: '#f8f9fa',
                padding: '15px',
                borderRadius: '8px',
                marginBottom: '20px',
                fontSize: '14px',
                color: '#6c757d'
              }}>
                <strong>Detalles:</strong><br />
                📅 {formatDate(selectedRequest.date)}<br />
                🕐 {formatTime(selectedRequest.startTime)} - {formatTime(selectedRequest.endTime)}<br />
                💰 {formatCurrency(selectedRequest.cost)}
              </div>
              
              {actionType === 'reject' && (
                <div className="form-group">
                  <label className="form-label">
                    Razón del rechazo *
                  </label>
                  <textarea
                    className="form-input"
                    value={reason}
                    onChange={(e) => setReason(e.target.value)}
                    placeholder="Ingrese la razón del rechazo (obligatorio)"
                    rows="4"
                    maxLength="500"
                    required
                  />
                  <small style={{ color: '#6c757d', fontSize: '12px' }}>
                    {reason.length}/500 caracteres
                  </small>
                </div>
              )}
              
              <div className="form-group">
                <label className="form-label">
                  Comentarios
                </label>
                <textarea
                  className="form-input"
                  value={comments}
                  onChange={(e) => setComments(e.target.value)}
                  placeholder="Ingrese un comentario (opcional)"
                  rows="4"
                  maxLength="500"
                />
                <small style={{ color: '#6c757d', fontSize: '12px' }}>
                  {comments.length}/500 caracteres
                </small>
              </div>
              
              <div className="modal-actions">
                <button 
                  className="btn-secondary"
                  onClick={handleClose}
                  disabled={isSubmitting}
                >
                  Cancelar
                </button>
                <button 
                  className="btn-primary"
                  onClick={handleConfirm}
                  disabled={isSubmitting || (actionType === 'reject' && !reason.trim())}
                >
                  {isSubmitting ? 'Procesando...' : 'Confirmar'}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      <Footer />
    </div>
  );
};

export default ConfirmationPage;

