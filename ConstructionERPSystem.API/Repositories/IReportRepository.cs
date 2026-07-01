using ConstructionERPSystem.API.Models;

namespace ConstructionERPSystem.API.Repositories
{
    public interface IReportRepository
    {
        ProjectSummaryReport GetProjectSummary();

        AttendanceSummaryReport GetAttendanceSummary();

        List<LowStockMaterialReport> GetLowStockMaterials();

        ExpenseSummaryReport GetExpenseSummary();

        InvoiceSummaryReport GetInvoiceSummary();
    }
}