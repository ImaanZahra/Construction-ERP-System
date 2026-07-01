using ConstructionERPSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        [HttpGet("project-summary")]
        public IActionResult GetProjectSummary()
        {
            return Ok(_service.GetProjectSummary());
        }

        [HttpGet("attendance-summary")]
        public IActionResult GetAttendanceSummary()
        {
            return Ok(_service.GetAttendanceSummary());
        }

        [HttpGet("low-stock-materials")]
        public IActionResult GetLowStockMaterials()
        {
            return Ok(_service.GetLowStockMaterials());
        }

        [HttpGet("expense-summary")]
        public IActionResult GetExpenseSummary()
        {
            return Ok(_service.GetExpenseSummary());
        }

        [HttpGet("invoice-summary")]
        public IActionResult GetInvoiceSummary()
        {
            return Ok(_service.GetInvoiceSummary());
        }
    }
}