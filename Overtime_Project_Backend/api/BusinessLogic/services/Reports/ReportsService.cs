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
        private readonly SMTPService _smtp;

        public ReportsService(OvertimeContext context, SMTPService smtp)
        {
            _context = context;
            _smtp = smtp;
        }

        // ======================================================
        // 🧾 Generar PDF
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

            var filePath = Path.Combine(Path.GetTempPath(), $"ReporteHoras_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            var document = new OvertimeReport(report);
            document.GeneratePdf(filePath);

            return filePath;
        }

        // ======================================================
        // ✉️ Enviar PDF al correo usando SMTPService
        // ======================================================
        public async Task<bool> SendReportAsync(string email)
        {
            var pdfPath = await GenerateReportAsync();
            var subject = "📊 Reporte de Horas Extra - Sistema Overtime";
            var message = "<p>Adjunto encontrarás tu reporte de métricas.</p>";

            try
            {
                // ✅ Llamamos al nuevo método que incluye adjuntos
                await _smtp.SendEmailWithAttachment(
                    email,
                    subject,
                    message,
                    pdfPath,
                    "ReporteHorasExtra.pdf"
                );

                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando reporte: {ex.Message}");
                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);
                return false;
            }
        }
    }
}
