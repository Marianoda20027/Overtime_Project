
import { httpService } from '../../services/http.service';
import { ErrorHandler } from '../../utilities/errorLoginHandler';


export const login = async (username, password) => {
  try {
    const payload = { username, password };
    const response = await httpService.post('/auth/login', payload);
    return response.data;
  } catch (error) {
    return { error: ErrorHandler.getErrorMessage(error) };
  }
};


export const verify2FA = async ({ Username, OTP }) => {
  try {
    const payload = { Username, OTP };
    const response = await httpService.post('/auth/2fa', payload);
    return response.data;
  } catch (error) {
    return { error: ErrorHandler.getErrorMessage(error) };
  }
};
