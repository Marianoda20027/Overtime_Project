// src/pages/OvertimePage.js
import React from "react";
import OvertimeRequest from "../../components/Overtime"; // Asegúrate de que la ruta sea correcta
import Header from "../../components/Header"; // Asegúrate de que la ruta sea correcta
import Footer from "../../components/Footer"; // Asegúrate de que la ruta sea correcta
import './styles.css'; // Estilos para la página

const OvertimePage = () => {
  return (
    <div className="overtime-page">
      {/* Header */}
      <Header />
      
      {/* Componente de Overtime */}
      <OvertimeRequest />
      
      {/* Footer */}
      <Footer />
    </div>
  );
};

export default OvertimePage;
