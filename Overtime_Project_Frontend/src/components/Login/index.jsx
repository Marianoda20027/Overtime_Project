import React, { useState } from 'react';
import { login, verify2FA } from './hooks';
import Cookies from 'js-cookie';
import { useNavigate } from 'react-router-dom';
import './styles.css';

const Login = () => {
  const [username, setUsername] = useState(''); // email
  const [password, setPassword] = useState('');
  const [otp, setOtp] = useState('');
  const [show2FA, setShow2FA] = useState(false);
  const [emailSentTo, setEmailSentTo] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [infoMessage, setInfoMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const navigate = useNavigate();

  // Paso 1: Login
  const handleLogin = async (e) => {
    e.preventDefault();
    setErrorMessage('');
    setInfoMessage('');

    if (!username || !password) {
      setErrorMessage('Email and password are required');
      return;
    }

    setIsLoading(true);
    const response = await login(username, password);
    setIsLoading(false);

    if (response.error) {
      setErrorMessage(response.error);
    } else if (response.message === 'Login successful. OTP sent.') {
      // Guardamos el email para el OTP
      setEmailSentTo(username); 
      setInfoMessage(`Login exitoso! Se ha enviado un código de verificación a tu correo: ${username}`);
      setShow2FA(true);
    } else {
      setInfoMessage(response.message);
    }
  };

  // Paso 2: Verificación 2FA
  const handle2FA = async (e) => {
    e.preventDefault();
    setErrorMessage('');
    setInfoMessage('');

    if (!otp) {
      setErrorMessage('Por favor ingresa el código recibido en tu correo.');
      return;
    }

    setIsLoading(true);
    const response = await verify2FA({ username: emailSentTo, otp }); // coincide con backend
    setIsLoading(false);

    if (response.error) {
      setErrorMessage(response.error);
    } else {
      // Guardar JWT en cookies
      if (response.token) {
        Cookies.set('jwt', response.token, { expires: 1 });
        const jwtToken = Cookies.get('jwt');
console.log(jwtToken); 
        navigate('/home'); // redirigir al dashboard
      }
    }
  };

  return (
    <div className="login-container">
      <div className="login-form">
        {!show2FA ? (
          <form onSubmit={handleLogin}>
            <h1>Login</h1>
            <div className="input-group">
              <label>Email</label>
              <input
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="initial.lastname@arkoselabs.com"
              />
            </div>

            <div className="input-group">
              <label>Password</label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Enter your password"
              />
            </div>

            {errorMessage && <div className="error-message">{errorMessage}</div>}
            {infoMessage && <div className="info-message">{infoMessage}</div>}

            <button type="submit" className="login-btn" disabled={isLoading}>
              {isLoading ? 'Loading...' : 'Login'}
            </button>
          </form>
        ) : (
          <form onSubmit={handle2FA}>
            <h1>Two-Factor Authentication</h1>
            <p>{infoMessage}</p>
            <div className="input-group">
              <label>2FA Code</label>
              <input
                type="text"
                value={otp}
                onChange={(e) => setOtp(e.target.value)}
                placeholder="Enter your code"
              />
            </div>

            {errorMessage && <div className="error-message">{errorMessage}</div>}

            <button type="submit" className="login-btn" disabled={isLoading}>
              {isLoading ? 'Verifying...' : 'Verify'}
            </button>
          </form>
        )}
      </div>
    </div>
  );
};

export default Login;
