using api.BusinessLogic.Services; 
using api.Data;
using api.Domain;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.IO;

namespace api.BusinessLogic.Services.Reports
{
    public class ReportsService
    {
        private readonly OvertimeContext _context;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _templateService;

        public ReportsService(OvertimeContext context, IEmailService emailService, IEmailTemplateService templateService)
        {
            _context = context;
            _emailService = emailService;
            _templateService = templateService;
        }

        // ======================================================
        // 🧾 Generate PDF Report
        // ======================================================
        public async Task<string> GenerateReportAsync()
        {
            var data = await _context.OvertimeRequests
                .Include(o => o.Approvals)
                .Include(o => o.User)
                .ToListAsync();

            var report = new ReportData
            {
                TotalRequests = data.Count,
                Approved = data.Count(r => r.Status == OvertimeStatus.Approved),
                Rejected = data.Count(r => r.Status == OvertimeStatus.Rejected),
                AvgResponseTime = data.Any(r => r.Approvals.Any())
                    ? data.Where(r => r.Approvals.Any())
                        .Average(r => (r.Approvals.First().ApprovalDate - r.CreatedAt).TotalHours)
                    : 0,
                TotalCost = data.Sum(r => r.Cost),
                TopUsers = data.GroupBy(r => r.User!.Email)
                               .Select(g => new TopUser
                               {
                                   UserName = g.Key,
                                   TotalHours = g.Sum(x => x.TotalHours)
                               })
                               .OrderByDescending(x => x.TotalHours)
                               .Take(5)
                               .ToList()
            };

            var filePath = Path.Combine(Path.GetTempPath(), $"OvertimeReport_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            var document = new OvertimeReport(report);
            document.GeneratePdf(filePath);

            return filePath;
        }

        // ======================================================
        // ✉️ Send PDF Report via Email with styled template
        // ======================================================
        public async Task<bool> SendReportAsync(string email)
        {
            var pdfPath = await GenerateReportAsync();
            var subject = "📊 Overtime Report - Monthly Analytics";
            
            // ✅ Use the beautiful styled template from EmailTemplateService
            var message = _templateService.GenerateReportEmail();

            try
            {
                // ✅ Send email with styled HTML and PDF attachment
                await _emailService.SendEmailWithAttachment(
                    email,
                    subject,
                    message,
                    pdfPath,
                    "OvertimeReport.pdf"
                );

                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending report: {ex.Message}");
                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);
                return false;
            }
        }
    }
}