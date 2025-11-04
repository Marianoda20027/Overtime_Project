import { httpService } from '../../services/http.service';
import { ErrorHandler } from '../../utilities/errorLoginHandler';
import toast from 'react-hot-toast';

export const login = async (username, password) => {
  try {
    const payload = { username, password };
    const response = await httpService.post('/auth/login', payload);
    
    // ✅ Toast de éxito cuando se envía el OTP
    if (response.data.message === 'Login successful. OTP sent.') {
      toast.success('Verification code sent to your email!', {
        duration: 4000,
        icon: '📧',
      });
    }
    
    return response.data;
  } catch (error) {
    const friendlyMessage = ErrorHandler.getErrorMessage(error);
    // ❌ Toast de error
    toast.error(friendlyMessage, {
      duration: 5000,
    });
    return { error: friendlyMessage };
  }
};

export const verify2FA = async ({ Username, OTP }) => {
  try {
    const payload = { Username, OTP };
    const response = await httpService.post('/auth/2fa', payload);
    
    // ✅ Toast de éxito cuando el 2FA es correcto
    if (response.data.token) {
      toast.success('Login successful! Redirecting...', {
        duration: 2000,
        icon: '✅',
      });
    }
    
    return response.data;
  } catch (error) {
    const friendlyMessage = ErrorHandler.getErrorMessage(error);
    // ❌ Toast de error
    toast.error(friendlyMessage, {
      duration: 5000,
    });
    return { error: friendlyMessage };
  }
};