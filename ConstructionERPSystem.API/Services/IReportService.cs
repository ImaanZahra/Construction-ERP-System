using ConstructionERPSystem.API.Models;

namespace ConstructionERPSystem.API.Services
{
    public interface IReportService
    {
        ProjectSummaryReport GetProjectSummary();

        AttendanceSummaryReport GetAttendanceSummary();

        List<LowStockMaterialReport> GetLowStockMaterials();

        ExpenseSummaryReport GetExpenseSummary();

        InvoiceSummaryReport GetInvoiceSummary();
    }
}