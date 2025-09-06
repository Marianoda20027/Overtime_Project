// src/pages/LoginPage.js
import React from "react";
import Header from "../../components/Header"; // Asegúrate de que la ruta sea correcta
import Login from "../../components/Login";
import Footer from "../../components/Footer"; // Asegúrate de que la ruta sea correcta
import './styles.css'; // Asegúrate de que el archivo CSS esté correctamente vinculado

const LoginPage = () => {
  return (
    <div className="login-page">
      {/* Integrando el header, login y footer */}
      <Header />
      <Login />
      <Footer />
    </div>
  );
};

export default LoginPage;
