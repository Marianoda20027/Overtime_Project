import React, { useState } from 'react';
import { login, verify2FA } from './hooks';
import './styles.css';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [twoFactor, setTwoFactor] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [show2FA, setShow2FA] = useState(false);

  // Validación tipo inicial.apellido@arkoselabs.com
  const emailPattern = /^[a-z]\.[a-z]+@arkoselabs\.com$/i;

  const handleLogin = async (e) => {
    e.preventDefault();
    setErrorMessage('');

    if (!emailPattern.test(username)) {
      setErrorMessage('El correo debe ser inicial.apellido@arkoselabs.com');
      return;
    }

    if (!password) {
      setErrorMessage('La contraseña es requerida');
      return;
    }

    setIsLoading(true);
    const response = await login(username, password);

    if (response.error) {
      setErrorMessage(response.error);
    } else {
      // Si el backend indica que requiere 2FA
      if (response.require2FA) {
        setShow2FA(true);
      } else {
        console.log('Login exitoso:', response);
      }
    }

    setIsLoading(false);
  };

  const handle2FA = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setErrorMessage('');

    if (!twoFactor) {
      setErrorMessage('Ingresa el código 2FA');
      setIsLoading(false);
      return;
    }

    const response = await verify2FA(twoFactor);

    if (response.error) {
      setErrorMessage(response.error);
    } else {
      console.log('2FA exitoso:', response);
      setShow2FA(false);
    }

    setIsLoading(false);
  };

  return (
    <div className="login-container">
      <div className="login-form">
        <h1>Arkose Labs Login</h1>

        {!show2FA ? (
          <form onSubmit={handleLogin}>
            <div className="input-group">
              <label htmlFor="username">Email</label>
              <input
                type="text"
                id="username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="inicial.apellido@arkoselabs.com"
              />
            </div>

            <div className="input-group">
              <label htmlFor="password">Password</label>
              <input
                type="password"
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Ingresa tu contraseña"
              />
            </div>

            {errorMessage && <div className="error-message">{errorMessage}</div>}

            <button type="submit" className="login-btn" disabled={isLoading}>
              {isLoading ? 'Cargando...' : 'Login'}
            </button>
          </form>
        ) : (
          <form onSubmit={handle2FA}>
            <div className="input-group">
              <label htmlFor="2fa">Código 2FA</label>
              <input
                type="text"
                id="2fa"
                value={twoFactor}
                onChange={(e) => setTwoFactor(e.target.value)}
                placeholder="Ingresa tu código"
              />
            </div>

            {errorMessage && <div className="error-message">{errorMessage}</div>}

            <button type="submit" className="login-btn" disabled={isLoading}>
              {isLoading ? 'Verificando...' : 'Verificar'}
            </button>
          </form>
        )}
      </div>
    </div>
  );
};

export default Login;
