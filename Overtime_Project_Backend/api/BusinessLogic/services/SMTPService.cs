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
        Task<bool> SendOvertimeNotificationAsync(string email, string subject, string message);
        Task<bool> SendEmailWithAttachment(string email, string subject, string message, string attachmentPath, string attachmentName);
    }

    public interface IEmailTemplateService
    {
        string GenerateTwoFactorEmail(string code);
        string GenerateOvertimeNotificationEmail(string title, string message, bool isApproved);
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

        public string GenerateOvertimeNotificationEmail(string title, string message, bool isApproved)
        {
            var color = isApproved ? "#50B95D" : "#dc3545";
            var statusText = isApproved ? "APROBADA" : "RECHAZADA";

            return $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<title>Notificación de Solicitud de Horas Extra</title>
<style>
  body {{
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background: #f4f4f4;
    margin: 0;
    padding: 20px;
  }}
  .email-container {{
    max-width: 650px;
    margin: 0 auto;
    background: #ffffff;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  }}
  .header {{
    background: {color};
    color: white;
    padding: 35px 30px;
    text-align: center;
  }}
  .header h1 {{
    font-size: 26px;
    margin: 0 0 12px 0;
    font-weight: 600;
  }}
  .content {{
    padding: 35px 30px;
  }}
  .footer {{
    background: #2c3e50;
    color: #ecf0f1;
    padding: 25px 30px;
    text-align: center;
    font-size: 12px;
  }}
</style>
</head>
<body>
  <div class='email-container'>
    <div class='header'>
      <h1>Solicitud {statusText}</h1>
    </div>
    <div class='content'>
      {message}
    </div>
    <div class='footer'>
      <strong>Sistema de Gestión de Horas Extra</strong><br/>
      Este es un mensaje automático, por favor no respondas.
    </div>
  </div>
</body>
</html>";
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

        // 🔐 Envío de código 2FA
        public async Task<bool> SendTwoFactorCodeAsync(string email, string code)
        {
            try
            {
                var smtpConfig = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpConfig["Email"];
                var password = smtpConfig["Password"];
                var host = smtpConfig["Host"];
                var port = int.Parse(smtpConfig["Port"] ?? "587");

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, "Sistema de Horas Extra"),
                    Subject = "Código de Autenticación",
                    Body = _templateService.GenerateTwoFactorEmail(code),
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                using var smtpClient = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando OTP");
                return false;
            }
        }

        // 📬 Notificación de solicitud aprobada o rechazada
        public async Task<bool> SendOvertimeNotificationAsync(string email, string subject, string message)
        {
            try
            {
                var smtpConfig = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpConfig["Email"];
                var password = smtpConfig["Password"];
                var host = smtpConfig["Host"];
                var port = int.Parse(smtpConfig["Port"] ?? "587");

                var isApproved = subject.Contains("Aprobada", StringComparison.OrdinalIgnoreCase);

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, "Sistema de Horas Extra"),
                    Subject = subject,
                    Body = _templateService.GenerateOvertimeNotificationEmail(subject, message, isApproved),
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                using var smtpClient = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación de horas extra");
                return false;
            }
        }

        // 📎 NUEVO → Enviar correo con adjunto (usado por ReportsService)
        public async Task<bool> SendEmailWithAttachment(string email, string subject, string message, string attachmentPath, string attachmentName)
        {
            try
            {
                var smtpConfig = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpConfig["Email"];
                var password = smtpConfig["Password"];
                var host = smtpConfig["Host"];
                var port = int.Parse(smtpConfig["Port"] ?? "587");

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, "Sistema de Horas Extra"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                if (File.Exists(attachmentPath))
                {
                    var attachment = new Attachment(attachmentPath);
                    attachment.Name = attachmentName;
                    mailMessage.Attachments.Add(attachment);
                }

                using var smtpClient = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Correo enviado con adjunto a {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo con adjunto a {Email}", email);
                return false;
            }
        }
    }
}
