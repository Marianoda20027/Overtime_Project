using System;
using System.Threading.Tasks;
using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.BusinessLogic.Services
{
    public class AuthService
    {
        private readonly OvertimeContext _context;
        private readonly IEmailService _emailService;

        public AuthService(OvertimeContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<(bool success, string message, string role)> AuthenticateUserAsync(string email, string password)
        {
            // Buscar en Users
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (user.PasswordHash != password)
                    return (false, "Invalid password", "");

                var otp = GenerateOTP();
                OTPStore.SetOTP(user.Email, otp, 10);
                var emailSent = await _emailService.SendTwoFactorCodeAsync(user.Email, otp);

                if (!emailSent)
                    return (false, "Failed to send OTP email", "");

                return (true, "Login successful. OTP sent.", "Employee");
            }

            // Buscar en Managers
            var manager = await _context.Managers.FirstOrDefaultAsync(m => m.Email.ToLower() == email.ToLower());
            if (manager != null)
            {
                if (manager.PasswordHash != password)
                    return (false, "Invalid password", "");

                var otp = GenerateOTP();
                OTPStore.SetOTP(manager.Email, otp, 10);
                var emailSent = await _emailService.SendTwoFactorCodeAsync(manager.Email, otp);

                if (!emailSent)
                    return (false, "Failed to send OTP email", "");

                return (true, "Login successful. OTP sent.", "Manager");
            }

            return (false, "User or Manager not found", "");
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
