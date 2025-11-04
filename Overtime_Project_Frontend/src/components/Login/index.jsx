import React, { useState } from 'react';
import { login, verify2FA } from './hooks';
import Cookies from 'js-cookie';
import { useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import './styles.css';
import { decodeJWT } from '../../hooks/decodeJWT.JSX';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [otp, setOtp] = useState('');
  const [show2FA, setShow2FA] = useState(false);
  const [emailSentTo, setEmailSentTo] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    // Validación básica con toast
    if (!username || !password) {
      toast.error('Email and password are required', {
        icon: '⚠️',
      });
      return;
    }

    setIsLoading(true);
    const response = await login(username, password);
    setIsLoading(false);

    // Si no hay error y el mensaje es correcto, mostrar 2FA
    if (!response.error && response.message === 'Login successful. OTP sent.') {
      setEmailSentTo(username);
      setShow2FA(true);
    }
  };

  const handle2FA = async (e) => {
    e.preventDefault();

    // Validación del OTP con toast
    if (!otp) {
      toast.error('Please enter the verification code', {
        icon: '⚠️',
      });
      return;
    }

    if (otp.length !== 6) {
      toast.error('Verification code must be 6 digits', {
        icon: '⚠️',
      });
      return;
    }

    setIsLoading(true);
    const response = await verify2FA({ Username: emailSentTo, OTP: otp });
    setIsLoading(false);

    // Si todo salió bien, navegar
    if (!response.error && response.token) {
      Cookies.set('jwt', response.token, { expires: 1 });
      const decoded = decodeJWT(response.token);
      const role = decoded?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      
      // Pequeño delay para que el usuario vea el toast de éxito
      setTimeout(() => {
        if (role === 'Employee' || role === 'Manager') {
          navigate('/home');
        } else {
          navigate('/reports');
        }
      }, 1000);
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
                disabled={isLoading}
              />
            </div>

            <div className="input-group">
              <label>Password</label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Enter your password"
                disabled={isLoading}
              />
            </div>

            <button type="submit" className="login-btn" disabled={isLoading}>
              {isLoading ? 'Loading...' : 'Login'}
            </button>
          </form>
        ) : (
          <form onSubmit={handle2FA}>
            <h1>Two-Factor Authentication</h1>
            <p className="info-text">
              A verification code has been sent to <strong>{emailSentTo}</strong>
            </p>
            <div className="input-group">
              <label>2FA Code</label>
              <input
                type="text"
                value={otp}
                onChange={(e) => setOtp(e.target.value)}
                placeholder="Enter your 6-digit code"
                maxLength={6}
                disabled={isLoading}
              />
            </div>

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