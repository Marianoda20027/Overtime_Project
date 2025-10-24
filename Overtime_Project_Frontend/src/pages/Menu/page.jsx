// src/pages/TwoButtonsPage.js
import React from 'react';
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import './styles.css';
import { decodeJWT } from '../../hooks/decodeJWT.JSX';

const TwoButtonsPage = () => {
  const navigate = useNavigate();

  // ✅ Leer token de las cookies
  const token = Cookies.get("jwt");
  const decoded = decodeJWT(token);
  const role = decoded?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

  const handleSubmitRequest = () => navigate('/overtime-request');
  const handleViewRequests = () => navigate('/request');
  const handleManagerRequests = () => navigate('/approval');

  return (
    <div className="buttons-page">
      <Header />

      <div className="content-container">
        <h2>Overtime Request System</h2>

        <div className="buttons-container">
          {role === "Manager" ? (
            
            <button
              className="button-center"
              onClick={handleManagerRequests}
              type="button"
              aria-label="View employees overtime requests"
            >
              Employees Requests
            </button>
          ) : (
        
            <>
              <button
                className="button-left"
                onClick={handleViewRequests}
                type="button"
                aria-label="View my previous overtime requests"
              >
                View My Request
              </button>

              <button
                className="button-right"
                onClick={handleSubmitRequest}
                type="button"
                aria-label="Submit a new overtime request"
              >
                Submit Request
              </button>
            </>
          )}
        </div>
      </div>

      <Footer />
    </div>
  );
};

export default TwoButtonsPage;
