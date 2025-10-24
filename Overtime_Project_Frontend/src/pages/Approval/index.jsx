import React, { useState } from 'react';
import { useRequests } from './hooks';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import './styles.css';

const ApprovalPage = () => {
  const { requests, acceptRequest, rejectRequest, loading } = useRequests();
  const [showModal, setShowModal] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState(null);
  const [actionType, setActionType] = useState(''); 
  const [reason, setReason] = useState(''); 
  const [comments, setComments] = useState(''); 
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
      // Calculate the overtime cost
      const cost = selectedRequest.totalHours * selectedRequest.salary; 

      if (actionType === 'accept') {
        await acceptRequest(selectedRequest.id, comments, cost);  
      } else if (actionType === 'reject') {
        if (!reason.trim()) {
          alert('Please provide a reason for rejection');
          setIsSubmitting(false);
          return;
        }
        await rejectRequest(selectedRequest.id, reason.trim(), comments, cost); 
      }

      handleClose();

    } catch (error) {
      console.error('Error processing the request:', error);
      alert('Error processing the request. Please try again.');
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

  // Loading state
  if (loading) {
    return (
      <div className="request-page">
        <Header />
        <h1 className="page-title">Pending Requests</h1>
        <div className="loading-container">
          <div className="loading-spinner"></div>
          <p className="loading-text">
            Loading requests...
          </p>
        </div>
        <Footer />
      </div>
    );
  }

  return (
    <div className="request-page">
      <Header />
      <h1 className="page-title">Pending Requests</h1>

      {requests.length === 0 ? (
        <div className="empty-state">
          <div className="empty-icon">📋</div>
          <h2 className="empty-title">No pending requests</h2>
          <p className="empty-message">
            New overtime requests will appear here for review.
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
                  <span className="detail-label">Schedule</span>
                  <div className="time-range">
                    <span>{formatTime(request.startTime)}</span>
                    <span>→</span>
                    <span>{formatTime(request.endTime)}</span>
                  </div>
                </div>
                
                <div className="justification">
                  <strong>Justification:</strong> {request.justification}
                </div>
                
                <div className="request-detail">
                  <span className="detail-label">Estimated Cost</span>
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
                  ✓ Accept
                </button>
                <button 
                  className="btn btn-reject"
                  onClick={() => handleReject(request)}
                  disabled={isSubmitting}
                >
                  ✗ Reject
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
                {actionType === 'accept' ? 'Accept Request' : 'Reject Request'}
              </h3>
            </div>
            
            <div className="modal-body">
              <p className="modal-text">
                {actionType === 'accept' 
                  ? `Are you sure you want to accept ${selectedRequest.person}'s request?`
                  : `Are you sure you want to reject ${selectedRequest.person}'s request?`
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
                <strong>Details:</strong><br />
                📅 {formatDate(selectedRequest.date)}<br />
                🕐 {formatTime(selectedRequest.startTime)} - {formatTime(selectedRequest.endTime)}<br />
                💰 {formatCurrency(selectedRequest.cost)}
              </div>
              
              {actionType === 'reject' && (
                <div className="form-group">
                  <label className="form-label">
                    Rejection Reason *
                  </label>
                  <textarea
                    className="form-input"
                    value={reason}
                    onChange={(e) => setReason(e.target.value)}
                    placeholder="Enter rejection reason (required)"
                    rows="4"
                    maxLength="500"
                    required
                  />
                  <small style={{ color: '#6c757d', fontSize: '12px' }}>
                    {reason.length}/500 characters
                  </small>
                </div>
              )}
              
              <div className="form-group">
                <label className="form-label">
                  Comments
                </label>
                <textarea
                  className="form-input"
                  value={comments}
                  onChange={(e) => setComments(e.target.value)}
                  placeholder="Enter a comment (optional)"
                  rows="4"
                  maxLength="500"
                />
                <small style={{ color: '#6c757d', fontSize: '12px' }}>
                  {comments.length}/500 characters
                </small>
              </div>
              
              <div className="modal-actions">
                <button 
                  className="btn-secondary"
                  onClick={handleClose}
                  disabled={isSubmitting}
                >
                  Cancel
                </button>
                <button 
                  className="btn-primary"
                  onClick={handleConfirm}
                  disabled={isSubmitting || (actionType === 'reject' && !reason.trim())}
                >
                  {isSubmitting ? 'Processing...' : 'Confirm'}
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

export default ApprovalPage;
