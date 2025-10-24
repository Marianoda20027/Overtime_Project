// src/pages/LoginPage.js
import React from "react";
import Header from "../../components/Header"; 
import Login from "../../components/Login";
import Footer from "../../components/Footer"; 
import './styles.css'; 

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
