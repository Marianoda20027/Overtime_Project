// src/pages/OvertimePage.js
import React from "react";
import OvertimeRequest from "../../components/Overtime"; 
import Header from "../../components/Header"; 
import Footer from "../../components/Footer"; 
import './styles.css'; 

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
