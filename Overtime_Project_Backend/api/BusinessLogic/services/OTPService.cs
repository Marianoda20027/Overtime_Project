using System;
using System.Collections.Concurrent;

namespace api.BusinessLogic.Services
{
    public static class OTPStore
    {
        private static readonly ConcurrentDictionary<string, (string Code, DateTime Expiry)> _otpDict = new();

        public static void SetOTP(string email, string code, int validMinutes = 10)
        {
            var expiry = DateTime.UtcNow.AddMinutes(validMinutes);
            _otpDict[email.ToLower()] = (code, expiry);
        }

        public static bool ValidateOTP(string email, string code)
        {
            email = email.ToLower();
            if (!_otpDict.TryGetValue(email, out var entry)) return false;

            if (entry.Code != code || DateTime.UtcNow > entry.Expiry)
            {
                _otpDict.TryRemove(email, out _);
                return false;
            }

            // OTP correcto, eliminar para no reutilizar
            _otpDict.TryRemove(email, out _);
            return true;
        }
    }
}
