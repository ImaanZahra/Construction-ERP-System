using ConstructionERPSystem.API.Data;
using ConstructionERPSystem.API.Models;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.API.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DbHelper _db;

        public ReportRepository(DbHelper db)
        {
            _db = db;
        }

        public ProjectSummaryReport GetProjectSummary()
        {
            var report = new ProjectSummaryReport();

            using var con = _db.GetConnection();
            con.Open();

            report.TotalProjects = GetCount(con, "SELECT COUNT(*) FROM Projects");
            report.ActiveProjects = GetCount(con, "SELECT COUNT(*) FROM Projects WHERE Status = 'Active'");
            report.CompletedProjects = GetCount(con, "SELECT COUNT(*) FROM Projects WHERE Status = 'Completed'");
            report.TotalSites = GetCount(con, "SELECT COUNT(*) FROM Sites");
            report.TotalTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks");

            return report;
        }

        public AttendanceSummaryReport GetAttendanceSummary()
        {
            var report = new AttendanceSummaryReport();

            using var con = _db.GetConnection();
            con.Open();

            report.TotalAttendanceRecords = GetCount(con, "SELECT COUNT(*) FROM Attendance");
            report.PresentCount = GetCount(con, "SELECT COUNT(*) FROM Attendance WHERE Status = 'Present'");
            report.AbsentCount = GetCount(con, "SELECT COUNT(*) FROM Attendance WHERE Status = 'Absent'");
            report.LeaveCount = GetCount(con, "SELECT COUNT(*) FROM Attendance WHERE Status = 'Leave'");

            return report;
        }

        public List<LowStockMaterialReport> GetLowStockMaterials()
        {
            var materials = new List<LowStockMaterialReport>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"SELECT MaterialId, MaterialName, Unit, CurrentStock, MinimumStock
                             FROM Materials
                             WHERE CurrentStock <= MinimumStock";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                materials.Add(new LowStockMaterialReport
                {
                    MaterialId = Convert.ToInt32(reader["MaterialId"]),
                    MaterialName = reader["MaterialName"].ToString(),
                    Unit = reader["Unit"].ToString(),
                    CurrentStock = Convert.ToInt32(reader["CurrentStock"]),
                    MinimumStock = Convert.ToInt32(reader["MinimumStock"])
                });
            }

            return materials;
        }

        public ExpenseSummaryReport GetExpenseSummary()
        {
            var report = new ExpenseSummaryReport();

            using var con = _db.GetConnection();
            con.Open();

            report.TotalExpenseRecords = GetCount(con, "SELECT COUNT(*) FROM Expenses");
            report.TotalExpenseAmount = GetDecimal(con, "SELECT ISNULL(SUM(Amount), 0) FROM Expenses");

            return report;
        }

        public InvoiceSummaryReport GetInvoiceSummary()
        {
            var report = new InvoiceSummaryReport();

            using var con = _db.GetConnection();
            con.Open();

            report.TotalInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices");
            report.PendingInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices WHERE Status = 'Pending'");
            report.PaidInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices WHERE Status = 'Paid'");
            report.TotalInvoiceAmount = GetDecimal(con, "SELECT ISNULL(SUM(TotalAmount), 0) FROM Invoices");
            report.TotalPaidAmount = GetDecimal(con, "SELECT ISNULL(SUM(PaidAmount), 0) FROM Invoices");

            return report;
        }

        private int GetCount(SqlConnection con, string query)
        {
            using SqlCommand cmd = new SqlCommand(query, con);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private decimal GetDecimal(SqlConnection con, string query)
        {
            using SqlCommand cmd = new SqlCommand(query, con);
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
    }
}        
