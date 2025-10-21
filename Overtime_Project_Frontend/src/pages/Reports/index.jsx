// src/pages/Reports/index.jsx
import React, { useState } from 'react';
import { useReport } from './hooks';
import './styles.css';

const ReportsPage = () => {
  const { pdfUrl, loading, message, error, generateAndSendReport } = useReport();
  const [email, setEmail] = useState('marianoda20027@gmail.com');

  const handleGenerate = async (e) => {
    e.preventDefault();
    await generateAndSendReport(email);
  };

  return (
    <div className="report-page">
      <div className="report-card">
        <h3>Overtime Report</h3>

        <form onSubmit={handleGenerate}>
          <div className="input-group">
            <label>Recipient Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              placeholder="example@email.com"
            />
          </div>

          <button className="report-btn" type="submit" disabled={loading}>
            {loading ? 'Generating...' : 'Generate and Send Report'}
          </button>
        </form>

        {message && <div className="success-message">{message}</div>}
        {error && <div className="error-message">{error}</div>}

        {pdfUrl && (
          <div className="report-viewer">
            <iframe
              src={pdfUrl}
              title="PDF Report"
              width="100%"
              height="600px"
              style={{ border: '1px solid #ccc', borderRadius: '10px' }}
            />
          </div>
        )}
      </div>
    </div>
  );
};

export default ReportsPage;