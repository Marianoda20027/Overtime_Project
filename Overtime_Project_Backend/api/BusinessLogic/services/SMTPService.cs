using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace api.BusinessLogic.Services
{
    // ------------------- Interfaces -------------------
    public interface IEmailService
    {
        Task<bool> SendTwoFactorCodeAsync(string email, string code);
    }

    public interface IEmailTemplateService
    {
        string GenerateTwoFactorEmail(string code);
    }

    // ------------------- Template Service -------------------
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GenerateTwoFactorEmail(string code)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<title>OTP Verification</title>
<style>
  body {{
    font-family: 'Open Sans', sans-serif;
    background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
    margin: 0;
    padding: 0;
  }}
  .email-container {{
    max-width: 600px;
    margin: 0 auto;
    background: #fff;
    padding: 40px;
    border-radius: 12px;
    box-shadow: 0 15px 40px rgba(50,50,93,0.1), 0 8px 20px rgba(0,0,0,0.1);
  }}
  h1 {{
    font-size: 32px;
    font-weight: 700;
    color: #50B95D;
    text-align: center;
  }}
  p {{
    font-size: 16px;
    color: #333;
    line-height: 1.6;
  }}
  .otp-box {{
    background-color: #f9f9f9;
    padding: 20px;
    text-align: center;
    font-size: 28px;
    font-weight: bold;
    letter-spacing: 5px;
    margin: 20px 0;
    border-radius: 10px;
    color: #50B95D;
  }}
  .footer {{
    font-size: 12px;
    color: #7f8c8d;
    text-align: center;
    margin-top: 20px;
  }}
</style>
</head>
<body>
  <div class='email-container'>
    <h1>Código de Autenticación</h1>
    <p>Hola,</p>
    <p>Tu código de verificación es:</p>
    <div class='otp-box'>{code}</div>
    <p><strong>Este código expirará en 10 minutos.</strong></p>
    <p>Si no solicitaste este código, ignora este correo.</p>
    <div class='footer'>Este es un correo automático, por favor no respondas.</div>
  </div>
</body>
</html>
";
        }
    }

    // ------------------- SMTP Service -------------------
    public class SMTPService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SMTPService> _logger;
        private readonly IEmailTemplateService _templateService;

        public SMTPService(IConfiguration configuration, ILogger<SMTPService> logger, IEmailTemplateService templateService)
        {
            _configuration = configuration;
            _logger = logger;
            _templateService = templateService;
        }

        public async Task<bool> SendTwoFactorCodeAsync(string email, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
                {
                    _logger.LogWarning("Invalid email or code");
                    return false;
                }

                var smtpConfig = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpConfig["Email"];
                var password = smtpConfig["Password"];
                var host = smtpConfig["Host"];
                var port = int.Parse(smtpConfig["Port"] ?? "587");

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, "MyApp Security"),
                    Subject = "Código de Autenticación",
                    Body = _templateService.GenerateTwoFactorEmail(code),
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                using var smtpClient = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 30000
                };

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("OTP sent to {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending OTP to {Email}", email);
                return false;
            }
        }
    }
}
