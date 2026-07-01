using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Repositories;

namespace ConstructionERPSystem.API.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public ProjectSummaryReport GetProjectSummary()
        {
            return _repository.GetProjectSummary();
        }

        public AttendanceSummaryReport GetAttendanceSummary()
        {
            return _repository.GetAttendanceSummary();
        }

        public List<LowStockMaterialReport> GetLowStockMaterials()
        {
            return _repository.GetLowStockMaterials();
        }

        public ExpenseSummaryReport GetExpenseSummary()
        {
            return _repository.GetExpenseSummary();
        }

        public InvoiceSummaryReport GetInvoiceSummary()
        {
            return _repository.GetInvoiceSummary();
        }
    }
}