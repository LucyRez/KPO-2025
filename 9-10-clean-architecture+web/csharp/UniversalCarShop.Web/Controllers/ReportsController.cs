using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalCarShop.UseCases.Reports;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportsController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet("[action]")]
        public IActionResult ExportJson()
        {
            using var writer = new StringWriter();
            _reportingService.ExportReport(ReportFormat.Json, writer);
            var fileBytes = Encoding.UTF8.GetBytes(writer.ToString());
            return File(fileBytes, "application/json", "report.json");
        }

        [HttpGet("[action]")]
        public IActionResult ExportMarkdown()
        {
            using var writer = new StringWriter();
            _reportingService.ExportReport(ReportFormat.Markdown, writer);
            var fileBytes = Encoding.UTF8.GetBytes(writer.ToString());
            return File(fileBytes, "text/markdown", "report.md");
        }

        [HttpPost("[action]")]
        public IActionResult ExportToServer()
        {
            _reportingService.ExportReport(ReportFormat.Server, null!);
            return Ok();
        }
    }
}
