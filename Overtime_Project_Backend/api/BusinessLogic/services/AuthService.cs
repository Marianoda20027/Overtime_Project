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

        public async Task<(bool success, string message)> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
                return (false, "User not found");

            if (user.PasswordHash != password)
                return (false, "Invalid password");

            // Generar OTP
            var otp = GenerateOTP();

            // Guardar OTP en memoria
            OTPStore.SetOTP(user.Email, otp, 10);

            // Enviar OTP
            var emailSent = await _emailService.SendTwoFactorCodeAsync(user.Email, otp);
            if (!emailSent)
                return (false, "Failed to send OTP email");

            return (true, "Login successful. OTP sent.");
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
