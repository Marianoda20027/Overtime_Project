// src/pages/TwoButtonsPage.js
import React from 'react';
import { useNavigate } from 'react-router-dom'; // Importamos el hook useNavigate para la navegación
import Header from "../../components/Header"; // Asegúrate de que la ruta sea correcta
import Footer from "../../components/Footer"; // Asegúrate de que la ruta sea correcta
import './styles.css'; // Importamos los estilos

const TwoButtonsPage = () => {
  const navigate = useNavigate(); // Usamos el hook useNavigate para navegar a otra página

  // Función para manejar la acción del botón "Submit Request"
  const handleSubmitRequest = () => {
    try {
      navigate('/overtime-request'); // Navegamos a la página OvertimeRequest
    } catch (error) {
      console.error('Error al navegar a overtime-request:', error);
    }
  };

  // Función para manejar la acción del botón "View My Request"
  const handleViewRequests = () => {
    try {
      // Navegamos a la página de visualización de requests
      navigate('/request'); // Cambiamos el console.log por una navegación real
    } catch (error) {
      console.error('Error al navegar a view-requests:', error);
      // Fallback: mostrar mensaje en consola si hay error
      console.log('Ver solicitudes anteriores - Error en navegación');
    }
  };

  return (
    <div className="buttons-page">
      <Header /> {/* Agregamos el Header */}
      
      <div className="content-container">
        {/* Título principal */}
        <h2>Overtime Request System</h2>
        
        
        {/* Botones a la izquierda y derecha */}
        <div className="buttons-container">
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
        </div>
      </div>

      <Footer /> {/* Agregamos el Footer */}
    </div>
  );
};

export default TwoButtonsPage;