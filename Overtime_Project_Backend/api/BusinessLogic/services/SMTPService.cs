using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

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
        string GenerateReportEmail();
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
    background: #f4f4f4;
    margin: 0;
    padding: 0;
  }}
  .container {{
    max-width: 600px;
    margin: 30px auto;
    background: #fff;
    border-radius: 10px;
    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
    padding: 40px;
  }}
  h1 {{
    text-align: center;
    color: #50B95D;
  }}
  .code {{
    background: #f0f0f0;
    font-size: 30px;
    text-align: center;
    padding: 15px;
    border-radius: 8px;
    font-weight: bold;
    letter-spacing: 6px;
  }}
  p {{
    color: #333;
  }}
  .footer {{
    text-align: center;
    color: #aaa;
    font-size: 12px;
    margin-top: 30px;
  }}
</style>
</head>
<body>
  <div class='container'>
    <h1>Authentication Code</h1>
    <p>Hello,</p>
    <p>Your verification code is:</p>
    <div class='code'>{code}</div>
    <p>This code will expire in 10 minutes.</p>
    <div class='footer'>This is an automated message, please do not reply.</div>
  </div>
</body>
</html>";
        }

        public string GenerateOvertimeNotificationEmail(string title, string message, bool isApproved)
        {
            var color = isApproved ? "#50B95D" : "#dc3545";
            var statusText = isApproved ? "APPROVED" : "REJECTED";

            return $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<title>Overtime Request Notification</title>
<style>
  body {{
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background: #f4f4f4;
    padding: 20px;
  }}
  .email {{
    max-width: 650px;
    margin: 0 auto;
    background: #fff;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
  }}
  .header {{
    background: {color};
    color: white;
    text-align: center;
    padding: 25px;
  }}
  .header h1 {{
    margin: 0;
  }}
  .content {{
    padding: 30px;
    color: #333;
  }}
  .footer {{
    background: #2c3e50;
    color: #ecf0f1;
    text-align: center;
    padding: 15px;
    font-size: 12px;
  }}
</style>
</head>
<body>
  <div class='email'>
    <div class='header'>
      <h1>REQUEST {statusText}</h1>
    </div>
    <div class='content'>
      <p>{message}</p>
    </div>
    <div class='footer'>
      <strong>Overtime Management System</strong><br/>
      This is an automated message, please do not reply.
    </div>
  </div>
</body>
</html>";
        }

        public string GenerateReportEmail() => "<p>Report template placeholder.</p>";
    }

    // ------------------- SendGrid Email Service -------------------
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

        private async Task<bool> SendViaSendGridAsync(string to, string subject, string html, string attachmentPath = null, string attachmentName = null)
        {
            var apiKey = _configuration["SENDGRID_API_KEY"];
var fromEmail = _configuration["SENDGRID_FROM"] ?? "no-reply@overtimeproject.com";
            var fromName = _configuration["SENDGRID_FROM_NAME"] ?? "Overtime System";

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("Falta la clave SENDGRID_API_KEY en configuración.");
                return false;
            }

            try
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, fromName);
                var toAddress = new EmailAddress(to);
                var msg = MailHelper.CreateSingleEmail(from, toAddress, subject, plainTextContent: null, htmlContent: html);

                if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
                {
                    var bytes = await File.ReadAllBytesAsync(attachmentPath);
                    var fileBase64 = Convert.ToBase64String(bytes);
                    msg.AddAttachment(attachmentName ?? Path.GetFileName(attachmentPath), fileBase64);
                }

                var response = await client.SendEmailAsync(msg);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Correo enviado correctamente via SendGrid a {To}", to);
                    return true;
                }

                _logger.LogWarning("Error al enviar correo via SendGrid. Código: {StatusCode}", response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo via SendGrid");
                return false;
            }
        }

        // SMTP de respaldo (opcional)
        private async Task<bool> SendViaSMTPAsync(string email, string subject, string html)
        {
            try
            {
                var smtp = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtp["Email"];
                var password = smtp["Password"];
                var host = smtp["Host"];
                var port = int.Parse(smtp["Port"] ?? "587");

                using var msg = new MailMessage
                {
                    From = new MailAddress(fromEmail!, "Overtime System"),
                    Subject = subject,
                    Body = html,
                    IsBodyHtml = true
                };
                msg.To.Add(email);

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                await client.SendMailAsync(msg);
                _logger.LogInformation("Correo enviado via SMTP a {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo via SMTP");
                return false;
            }
        }

        // ------------------- Métodos Públicos -------------------
        public async Task<bool> SendTwoFactorCodeAsync(string email, string code)
        {
            var html = _templateService.GenerateTwoFactorEmail(code);
            var subject = "Authentication Code";

            if (await SendViaSendGridAsync(email, subject, html))
                return true;

            return await SendViaSMTPAsync(email, subject, html);
        }

        public async Task<bool> SendOvertimeNotificationAsync(string email, string subject, string message)
        {
            var isApproved = subject.Contains("Approved", StringComparison.OrdinalIgnoreCase);
            var html = _templateService.GenerateOvertimeNotificationEmail(subject, message, isApproved);

            if (await SendViaSendGridAsync(email, subject, html))
                return true;

            return await SendViaSMTPAsync(email, subject, html);
        }

        public async Task<bool> SendEmailWithAttachment(string email, string subject, string message, string attachmentPath, string attachmentName)
        {
            if (await SendViaSendGridAsync(email, subject, message, attachmentPath, attachmentName))
                return true;

            return await SendViaSMTPAsync(email, subject, message);
        }
    }
}
