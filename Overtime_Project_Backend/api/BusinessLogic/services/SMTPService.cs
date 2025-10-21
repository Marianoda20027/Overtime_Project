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
    <h1>Authentication Code</h1>
    <p>Hello,</p>
    <p>Your verification code is:</p>
    <div class='otp-box'>{code}</div>
    <p><strong>This code will expire in 10 minutes.</strong></p>
    <p>If you didn't request this code, please ignore this email.</p>
    <div class='footer'>This is an automated email, please do not reply.</div>
  </div>
</body>
</html>
";
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
      <h1>Request {statusText}</h1>
    </div>
    <div class='content'>
      {message}
    </div>
    <div class='footer'>
      <strong>Overtime Management System</strong><br/>
      This is an automated message, please do not reply.
    </div>
  </div>
</body>
</html>";
        }

        public string GenerateReportEmail()
        {
            return @"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<title>Overtime Report</title>
<style>
  body {
    font-family: 'Open Sans', sans-serif;
    background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
    margin: 0;
    padding: 0;
  }
  .email-container {
    max-width: 600px;
    margin: 0 auto;
    background: #fff;
    padding: 40px;
    border-radius: 12px;
    box-shadow: 0 15px 40px rgba(50,50,93,0.1), 0 8px 20px rgba(0,0,0,0.1);
  }
  h1 {
    font-size: 32px;
    font-weight: 700;
    color: #50B95D;
    text-align: center;
    margin: 0 0 10px 0;
  }
  .subtitle {
    font-size: 16px;
    color: #666;
    text-align: center;
    margin: 0 0 30px 0;
  }
  p {
    font-size: 16px;
    color: #333;
    line-height: 1.6;
    margin: 0 0 15px 0;
  }
  .report-box {
    background-color: #f9f9f9;
    padding: 25px;
    margin: 25px 0;
    border-radius: 10px;
    border-left: 4px solid #50B95D;
  }
  .report-box h2 {
    font-size: 20px;
    color: #50B95D;
    margin: 0 0 15px 0;
    font-weight: 600;
  }
  .report-box p {
    margin: 0 0 10px 0;
    font-size: 15px;
    color: #555;
  }
  .report-box p:last-child {
    margin: 0;
  }
  ul {
    color: #555;
    line-height: 1.8;
    padding-left: 20px;
    margin: 15px 0;
  }
  ul li {
    margin-bottom: 8px;
  }
  .footer {
    font-size: 12px;
    color: #7f8c8d;
    text-align: center;
    margin-top: 30px;
    padding-top: 20px;
    border-top: 1px solid #e0e0e0;
  }
  .footer strong {
    display: block;
    font-size: 14px;
    color: #333;
    margin-bottom: 5px;
  }
</style>
</head>
<body>
  <div class='email-container'>
    <h1>📊 Overtime Report</h1>
    <p class='subtitle'>Your monthly metrics and analytics</p>
    
    <p>Hello,</p>
    <p>We're pleased to share your <strong>Overtime Management Report</strong>. The attached PDF contains detailed metrics and insights for this period.</p>
    
    <div class='report-box'>
      <h2>📎 Attached Document</h2>
      <p><strong>OvertimeReport.pdf</strong></p>
      <p>Complete analysis of overtime requests, approvals, costs, and trends.</p>
    </div>

    <p><strong>This report includes:</strong></p>
    <ul>
      <li>Total requests submitted and processed</li>
      <li>Approval and rejection statistics</li>
      <li>Average response time analysis</li>
      <li>Total cost breakdown</li>
      <li>Top users by overtime hours</li>
    </ul>

    <p>If you have any questions or need additional information, please don't hesitate to reach out to our support team.</p>
    
    <div class='footer'>
      <strong>Overtime Management System</strong>
      <p>This is an automated email, please do not reply.</p>
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

        // 🔐 Send 2FA code
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
                    From = new MailAddress(fromEmail!, "Overtime System"),
                    Subject = "Authentication Code",
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
                _logger.LogError(ex, "Error sending OTP");
                return false;
            }
        }

        // 📬 Overtime notification (approved or rejected)
        public async Task<bool> SendOvertimeNotificationAsync(string email, string subject, string message)
        {
            try
            {
                var smtpConfig = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpConfig["Email"];
                var password = smtpConfig["Password"];
                var host = smtpConfig["Host"];
                var port = int.Parse(smtpConfig["Port"] ?? "587");

                var isApproved = subject.Contains("Approved", StringComparison.OrdinalIgnoreCase);

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, "Overtime System"),
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
                _logger.LogError(ex, "Error sending overtime notification");
                return false;
            }
        }

        // 📎 NEW → Send email with attachment (used by ReportsService)
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
                    From = new MailAddress(fromEmail!, "Overtime System"),
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
                _logger.LogInformation("Email sent with attachment to {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email with attachment to {Email}", email);
                return false;
            }
        }
    }
}