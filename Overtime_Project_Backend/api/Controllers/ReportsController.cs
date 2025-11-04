using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.BusinessLogic.Services.Reports;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] 
    public class ReportsController : ControllerBase
    {
        private readonly ReportsService _reports;

        public ReportsController(ReportsService reports)
        {
            _reports = reports;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GenerateReport()
        {
            try
            {
                var path = await _reports.GenerateReportAsync();

                if (!System.IO.File.Exists(path))
                    return NotFound(new { message = "❌ Could not generate PDF file (possibly no requests registered)." });

                var bytes = await System.IO.File.ReadAllBytesAsync(path);
                System.IO.File.Delete(path);

                return File(bytes, "application/pdf", "OvertimeReport.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
                return StatusCode(500, new { message = $"Error generating report: {ex.Message}" });
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendReport([FromBody] ReportEmailRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest(new { message = "Email address is required." });

                var ok = await _reports.SendReportAsync(request.Email);

                if (ok)
                    return Ok(new { message = $"✅ Report sent successfully to {request.Email}" });
                else
                    return StatusCode(500, new { message = "❌ Error sending report." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending report: {ex.Message}");
                return StatusCode(500, new { message = $"Error sending report: {ex.Message}" });
            }
        }
    }

    public class ReportEmailRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}