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
            var statusLabel = isApproved ? "Aprobada" : "Rechazada";

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
    letter-spacing: 0.5px;
  }}
  .status-badge {{
    display: inline-block;
    background: rgba(255,255,255,0.2);
    color: white;
    padding: 6px 18px;
    border-radius: 4px;
    font-weight: 500;
    font-size: 13px;
    text-transform: uppercase;
    letter-spacing: 1px;
  }}
  .content {{
    padding: 35px 30px;
  }}
  .greeting {{
    font-size: 16px;
    color: #333;
    margin-bottom: 20px;
    line-height: 1.6;
  }}
  .info-section {{
    background: #f8f9fa;
    border-left: 4px solid {color};
    padding: 25px;
    margin: 25px 0;
    border-radius: 4px;
  }}
  .info-section h3 {{
    margin: 0 0 15px 0;
    color: #2c3e50;
    font-size: 16px;
    font-weight: 600;
  }}
  .info-row {{
    display: flex;
    padding: 8px 0;
    border-bottom: 1px solid #e9ecef;
  }}
  .info-row:last-child {{
    border-bottom: none;
  }}
  .info-label {{
    font-weight: 600;
    color: #495057;
    min-width: 140px;
    font-size: 14px;
  }}
  .info-value {{
    color: #212529;
    font-size: 14px;
  }}
  .note {{
    margin-top: 25px;
    padding: 15px;
    background: #fff3cd;
    border-left: 4px solid #ffc107;
    border-radius: 4px;
    font-size: 14px;
    color: #856404;
  }}
  .footer {{
    background: #2c3e50;
    color: #ecf0f1;
    padding: 25px 30px;
    text-align: center;
    font-size: 12px;
    line-height: 1.8;
  }}
  .footer-divider {{
    height: 1px;
    background: rgba(255,255,255,0.1);
    margin: 15px 0;
  }}
</style>
</head>
<body>
  <div class='email-container'>
    <div class='header'>
      <h1>Notificación de Solicitud de Horas Extra</h1>
      <div class='status-badge'>Estado: {statusText}</div>
    </div>
    
    <div class='content'>
      <div class='greeting'>
        <p>Estimado colaborador,</p>
        <p>Le informamos que su solicitud de horas extra ha sido <strong>{statusLabel.ToLower()}</strong>. A continuación encontrará los detalles:</p>
      </div>
      
      <div class='info-section'>
        <h3>Detalles de la Solicitud</h3>
        {message}
      </div>
      
      <div class='note'>
        <strong>Nota:</strong> Para cualquier consulta sobre esta solicitud, por favor contacte a su supervisor directo.
      </div>
    </div>
    
    <div class='footer'>
      <strong>Sistema de Gestión de Horas Extra</strong>
      <div class='footer-divider'></div>
      Este es un mensaje automático generado por el sistema. Por favor no responda a este correo.
      <br>© 2025 Departamento de Recursos Humanos
    </div>
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
                    From = new MailAddress(fromEmail, "Sistema de Horas Extra"),
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

        public async Task<bool> SendOvertimeNotificationAsync(string email, string subject, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    _logger.LogWarning("Invalid email");
                    return false;
                }

                // 🔥 Detectar si es aprobada buscando "Aprobada" (case insensitive)
                var isApproved = subject.Contains("Aprobada", StringComparison.OrdinalIgnoreCase);
                var title = isApproved ? "Solicitud Aprobada" : "Solicitud Rechazada";

                var smtpConfig = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpConfig["Email"];
                var password = smtpConfig["Password"];
                var host = smtpConfig["Host"];
                var port = int.Parse(smtpConfig["Port"] ?? "587");

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, "Sistema de Horas Extra"),
                    Subject = subject,
                    Body = _templateService.GenerateOvertimeNotificationEmail(title, message, isApproved),
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
                _logger.LogInformation("Overtime notification sent to {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification to {Email}", email);
                return false;
            }
        }
    }
}