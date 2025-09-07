// src/pages/YourRequests/index.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useYourRequests } from './hooks.jsx';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import './styles.css';

const YourRequests = () => {
  const navigate = useNavigate();
  const { requests, loading, error } = useYourRequests();

  const handleGoBack = () => {
    navigate('/');
  };

  const handleNewRequest = () => {
    navigate('/overtime-request');
  };

  return (
    <div className="your-requests-page">
      <Header />
      
      <div className="requests-container">
        <div className="requests-header">
          <h2>Your Overtime Requests</h2>
          <p>Manage and track all your overtime requests</p>
        </div>

        <div className="action-buttons">
          <button className="btn-secondary" onClick={handleGoBack}>
            ← Back to Home
          </button>
          <button className="btn-primary" onClick={handleNewRequest}>
            + New Request
          </button>
        </div>

        {loading && <div className="loading">Loading...</div>}
        
        {error && <div className="error">{error}</div>}

        {!loading && !error && (
          <div className="requests-list">
            {requests.length === 0 ? (
              <div className="no-requests">
                <h3>No Requests Found</h3>
                <p>You haven't submitted any overtime requests yet.</p>
                <button className="btn-primary" onClick={handleNewRequest}>
                  Create Your First Request
                </button>
              </div>
            ) : (
              <table className="requests-table">
                <thead>
                  <tr>
                    <th>Date</th>
                    <th>Time</th>
                    <th>Hours</th>
                    <th>Status</th>
                    <th>Justification</th>
                  </tr>
                </thead>
                <tbody>
                  {requests.map((request) => (
                    <tr key={request.id}>
                      <td>{request.date}</td>
                      <td>{request.startTime} - {request.endTime}</td>
                      <td>{request.totalHours}h</td>
                      <td>
                        <span className={`status ${request.status?.toLowerCase()}`}>
                          {request.status}
                        </span>
                      </td>
                      <td>{request.justification}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        )}
      </div>

      <Footer />
    </div>
  );
};

export default YourRequests;