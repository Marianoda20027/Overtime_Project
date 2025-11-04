// src/pages/Reports/index.jsx
import React, { useState } from 'react';
import { useReport } from './hooks';
import './styles.css';

const ReportsPage = () => {
  const { pdfUrl, loading, generatingPdf, sendingEmail, generateAndSendReport } = useReport();
  const [email, setEmail] = useState('');

  const handleGenerate = async (e) => {
    e.preventDefault();
    await generateAndSendReport(email);
  };

  const handleDownload = () => {
    if (pdfUrl) {
      const link = document.createElement('a');
      link.href = pdfUrl;
      link.download = 'OvertimeReport.pdf';
      link.click();
    }
  };

  return (
    <div className="report-page">
      <div className="report-card">
        <h3>📊 Overtime Report</h3>
        <p className="report-subtitle">Generate and send detailed overtime reports</p>

        <form onSubmit={handleGenerate}>
          <div className="input-group">
            <label>Recipient Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              placeholder="recipient@example.com"
              disabled={loading}
            />
            <small className="input-hint">
              The report will be sent to this email address
            </small>
          </div>

          {/* 💬 Mensaje en DOM - Estado de procesamiento */}
          {loading && (
            <div className="processing-status">
              {generatingPdf && (
                <div className="status-item">
                  <span className="spinner"></span>
                  <span>Generating PDF report...</span>
                </div>
              )}
              {sendingEmail && (
                <div className="status-item">
                  <span className="spinner"></span>
                  <span>Sending email to {email}...</span>
                </div>
              )}
            </div>
          )}

          <div className="button-group">
            <button className="report-btn" type="submit" disabled={loading}>
              {loading ? (
                <>
                  <span className="spinner"></span>
                  Processing...
                </>
              ) : (
                <>
                  <span>📧</span>
                  Generate & Send Report
                </>
              )}
            </button>

        
          </div>
        </form>

        {/* 💬 Mensaje en DOM - PDF disponible */}
        {pdfUrl && !loading && (
          <div className="success-info">
            <div className="info-text">
              <strong>Report ready!</strong>
              <p>You can download the PDF or send it to another email address.</p>
            </div>
          </div>
        )}

        {/* Preview del PDF */}
        {pdfUrl && (
          <div className="report-viewer">
            <h4>Preview</h4>
            <iframe
              src={pdfUrl}
              title="PDF Report"
              width="100%"
              height="600px"
              style={{ border: '1px solid #e0e0e0', borderRadius: '10px' }}
            />
          </div>
        )}
      </div>
    </div>
  );
};

export default ReportsPage;