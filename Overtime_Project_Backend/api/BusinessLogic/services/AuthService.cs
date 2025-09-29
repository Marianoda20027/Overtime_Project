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

        // Autenticar usuario y enviar OTP
        public async Task<(bool success, string message)> AuthenticateUserAsync(string email, string password)
        {
            // Buscar usuario en la base de datos
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return (false, "User not found");

            // Comparar contraseña en texto plano
            if (user.PasswordHash != password)
                return (false, "Invalid password");

            // Generar OTP manual (6 dígitos)
            var otp = GenerateOTP();

            // Enviar OTP por correo
            var emailSent = await _emailService.SendTwoFactorCodeAsync(user.Email, otp);
            if (!emailSent)
                return (false, "Failed to send OTP email");

            return (true, "Login successful. OTP sent.");
        }

        // Generar un OTP de 6 dígitos
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
