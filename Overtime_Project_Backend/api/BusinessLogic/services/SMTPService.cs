using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace api.BusinessLogic.Services
{
    public interface IEmailService
    {
        Task<bool> SendTwoFactorCodeAsync(string email, string code);
    }

    public class SMTPService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SMTPService> _logger;

        public SMTPService(IConfiguration configuration, ILogger<SMTPService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendTwoFactorCodeAsync(string email, string code)
        {
            try
            {
                // Validar email de entrada
                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                {
                    _logger.LogWarning("Invalid email address provided: {Email}", email);
                    return false;
                }

                // Validar código
                if (string.IsNullOrWhiteSpace(code))
                {
                    _logger.LogWarning("Invalid OTP code provided");
                    return false;
                }

                var smtpConfig = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpConfig["Email"];
                var password = smtpConfig["Password"];
                var host = smtpConfig["Host"];
                var portString = smtpConfig["Port"];

                // Validar configuración completa
                if (string.IsNullOrEmpty(fromEmail) || 
                    string.IsNullOrEmpty(password) || 
                    string.IsNullOrEmpty(host) ||
                    string.IsNullOrEmpty(portString))
                {
                    _logger.LogError("SMTP configuration is incomplete");
                    return false;
                }

                if (!int.TryParse(portString, out int port))
                {
                    _logger.LogError("Invalid SMTP port configuration");
                    return false;
                }

                // Crear mensaje
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, "MyApp Security"),
                    Subject = "Your Two-Factor Authentication Code",
                    Body = GenerateEmailBody(code),
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                };
                mailMessage.To.Add(email);

                // Configurar SMTP client
                using var smtpClient = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 30000
                };

                await smtpClient.SendMailAsync(mailMessage);
                
                _logger.LogInformation("OTP sent successfully to {Email}", MaskEmail(email));
                return true;
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "SMTP error sending OTP to {Email}: {StatusCode}", 
                    MaskEmail(email), smtpEx.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending OTP to {Email}", MaskEmail(email));
                return false;
            }
        }

        private string GenerateEmailBody(string code)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
</head>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <h2 style='color: #2c3e50;'>Código de Autenticación</h2>
        <p>Hola,</p>
        <p>Tu código de verificación es:</p>
        <div style='background-color: #f4f4f4; padding: 15px; text-align: center; font-size: 24px; font-weight: bold; letter-spacing: 5px; margin: 20px 0;'>
            {code}
        </div>
        <p style='color: #e74c3c;'><strong>Este código expirará en 10 minutos.</strong></p>
        <p>Si no solicitaste este código, por favor ignora este correo.</p>
        <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'>
        <p style='color: #7f8c8d; font-size: 12px;'>
            Este es un correo automático, por favor no respondas a este mensaje.
        </p>
    </div>
</body>
</html>";
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return "unknown";
            
            var parts = email.Split('@');
            if (parts.Length != 2) return "invalid";
            
            var localPart = parts[0];
            var domain = parts[1];
            
            if (localPart.Length <= 2)
                return $"{localPart[0]}***@{domain}";
            
            return $"{localPart[0]}***{localPart[^1]}@{domain}";
        }
    }
}
