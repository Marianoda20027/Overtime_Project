using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.BusinessLogic.Services.Reports;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // 🔓 Permitir acceso sin token JWT (solo para pruebas o entornos de desarrollo)
    public class ReportsController : ControllerBase
    {
        private readonly ReportsService _reports;

        public ReportsController(ReportsService reports)
        {
            _reports = reports;
        }

        // ======================================================
        // 🧾 GET: Generar reporte PDF
        // ======================================================
        [HttpGet("generate")]
        public async Task<IActionResult> GenerateReport()
        {
            try
            {
                var path = await _reports.GenerateReportAsync();

                if (!System.IO.File.Exists(path))
                    return NotFound(new { message = "❌ No se pudo generar el archivo PDF (posiblemente no hay solicitudes registradas)." });

                var bytes = await System.IO.File.ReadAllBytesAsync(path);
                System.IO.File.Delete(path);

                return File(bytes, "application/pdf", "ReporteHorasExtra.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generando reporte: {ex.Message}");
                return StatusCode(500, new { message = $"Error generando el reporte: {ex.Message}" });
            }
        }

        // ======================================================
        // ✉️ POST: Enviar reporte al correo (adjunto PDF)
        // ======================================================
        [HttpPost("send")]
        public async Task<IActionResult> SendReport([FromBody] ReportEmailRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest(new { message = "El correo electrónico es obligatorio." });

                var ok = await _reports.SendReportAsync(request.Email);

                if (ok)
                    return Ok(new { message = $"✅ Reporte enviado correctamente a {request.Email}" });
                else
                    return StatusCode(500, new { message = "❌ Error enviando el reporte." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando reporte: {ex.Message}");
                return StatusCode(500, new { message = $"Error enviando el reporte: {ex.Message}" });
            }
        }
    }

    public class ReportEmailRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}
