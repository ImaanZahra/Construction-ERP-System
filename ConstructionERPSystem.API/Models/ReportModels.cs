namespace ConstructionERPSystem.API.Models
{
    public class ProjectSummaryReport
    {
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int TotalSites { get; set; }
        public int TotalTasks { get; set; }
    }

    public class AttendanceSummaryReport
    {
        public int TotalAttendanceRecords { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LeaveCount { get; set; }
    }

    public class LowStockMaterialReport
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string Unit { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
    }

    public class ExpenseSummaryReport
    {
        public int TotalExpenseRecords { get; set; }
        public decimal TotalExpenseAmount { get; set; }
    }

    public class InvoiceSummaryReport
    {
        public int TotalInvoices { get; set; }
        public int PendingInvoices { get; set; }
        public int PaidInvoices { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
    }
}