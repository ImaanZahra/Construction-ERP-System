using ConstructionERPSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DbHelper _db;

        public DashboardController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalClients =
                GetCount(con, "SELECT COUNT(*) FROM Clients");

            ViewBag.TotalProjects =
                GetCount(con, "SELECT COUNT(*) FROM Projects");

            ViewBag.TotalEmployees =
                GetCount(con, "SELECT COUNT(*) FROM Employees");

            ViewBag.TotalTasks =
                GetCount(con, "SELECT COUNT(*) FROM Tasks");

            return View();
        }

        private int GetCount(SqlConnection con, string query)
        {
            using SqlCommand cmd = new SqlCommand(query, con);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public IActionResult Admin()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalUsers = GetCount(con, "SELECT COUNT(*) FROM Users");
            ViewBag.TotalClients = GetCount(con, "SELECT COUNT(*) FROM Clients");
            ViewBag.TotalProjects = GetCount(con, "SELECT COUNT(*) FROM Projects");
            ViewBag.TotalEmployees = GetCount(con, "SELECT COUNT(*) FROM Employees");
            ViewBag.TotalTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks");
            ViewBag.TotalMaterials = GetCount(con, "SELECT COUNT(*) FROM Materials");
            ViewBag.TotalVendors = GetCount(con, "SELECT COUNT(*) FROM Vendors");
            ViewBag.TotalInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices");

            return View();
        }

        public IActionResult ProjectManager()
        {
            if (HttpContext.Session.GetString("Role") != "Project Manager")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalProjects = GetCount(con, "SELECT COUNT(*) FROM Projects");
            ViewBag.TotalSites = GetCount(con, "SELECT COUNT(*) FROM Sites");
            ViewBag.PendingTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Pending'");
            ViewBag.CompletedTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Completed'");
            ViewBag.TotalEmployees = GetCount(con, "SELECT COUNT(*) FROM Employees");
            ViewBag.TotalExpenses = GetCount(con, "SELECT COUNT(*) FROM Expenses");

            return View();
        }

        public IActionResult Employee()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null)
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks");
            ViewBag.PendingTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Pending'");
            ViewBag.CompletedTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Completed'");
            ViewBag.AttendanceRecords = GetCount(con, "SELECT COUNT(*) FROM Attendance");

            return View();
        }

        public IActionResult Client()
        {
            if (HttpContext.Session.GetString("Role") != "Client")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalProjects = GetCount(con, "SELECT COUNT(*) FROM Projects");
            ViewBag.TotalInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices");
            ViewBag.PendingInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices WHERE Status = 'Pending'");
            ViewBag.PaidInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices WHERE Status = 'Paid'");

            return View();
        }
        public IActionResult SiteEngineer()
        {
            if (HttpContext.Session.GetString("Role") != "Site Engineer")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalSites = GetCount(con, "SELECT COUNT(*) FROM Sites");
            ViewBag.TotalTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks");
            ViewBag.PendingTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Pending'");
            ViewBag.AttendanceRecords = GetCount(con, "SELECT COUNT(*) FROM Attendance");

            return View();
        }
        public IActionResult StoreManager()
        {
            if (HttpContext.Session.GetString("Role") != "Store Manager")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalMaterials = GetCount(con, "SELECT COUNT(*) FROM Materials");
            ViewBag.LowStockMaterials = GetCount(con, "SELECT COUNT(*) FROM Materials WHERE CurrentStock <= MinimumStock");
            ViewBag.TotalVendors = GetCount(con, "SELECT COUNT(*) FROM Vendors");
            ViewBag.PurchaseOrders = GetCount(con, "SELECT COUNT(*) FROM PurchaseOrders");
            ViewBag.PendingOrders = GetCount(con, "SELECT COUNT(*) FROM PurchaseOrders WHERE Status = 'Pending'");

            return View();
        }
        public IActionResult ProcurementOfficer()
        {
            if (HttpContext.Session.GetString("Role") != "Procurement Officer")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalVendors = GetCount(con, "SELECT COUNT(*) FROM Vendors");
            ViewBag.TotalMaterials = GetCount(con, "SELECT COUNT(*) FROM Materials");
            ViewBag.TotalPurchaseOrders = GetCount(con, "SELECT COUNT(*) FROM PurchaseOrders");
            ViewBag.PendingOrders = GetCount(con, "SELECT COUNT(*) FROM PurchaseOrders WHERE Status = 'Pending'");
            ViewBag.ApprovedOrders = GetCount(con, "SELECT COUNT(*) FROM PurchaseOrders WHERE Status = 'Approved'");
            ViewBag.DeliveredOrders = GetCount(con, "SELECT COUNT(*) FROM PurchaseOrders WHERE Status = 'Delivered'");

            return View();
        }
        public IActionResult HRManager()
        {
            if (HttpContext.Session.GetString("Role") != "HR Manager")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalEmployees = GetCount(con, "SELECT COUNT(*) FROM Employees");
            ViewBag.TotalAttendance = GetCount(con, "SELECT COUNT(*) FROM Attendance");
            ViewBag.PresentToday = GetCount(con, "SELECT COUNT(*) FROM Attendance WHERE Status = 'Present'");
            ViewBag.AbsentToday = GetCount(con, "SELECT COUNT(*) FROM Attendance WHERE Status = 'Absent'");

            return View();
        }
        public IActionResult Accountant()
        {
            if (HttpContext.Session.GetString("Role") != "Accountant")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalExpenses = GetCount(con, "SELECT COUNT(*) FROM Expenses");
            ViewBag.TotalInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices");
            ViewBag.PendingInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices WHERE Status = 'Pending'");
            ViewBag.PaidInvoices = GetCount(con, "SELECT COUNT(*) FROM Invoices WHERE Status = 'Paid'");

            return View();
        }
        public IActionResult Contractor()
        {
            if (HttpContext.Session.GetString("Role") != "Contractor")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks");
            ViewBag.PendingTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Pending'");
            ViewBag.CompletedTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Completed'");
            ViewBag.TotalWorkers = GetCount(con, "SELECT COUNT(*) FROM Employees WHERE EmployeeType = 'Worker'");

            return View();
        }
        public IActionResult Worker()
        {
            if (HttpContext.Session.GetString("Role") != "Worker")
                return RedirectToAction("Login", "Auth");

            using var con = _db.GetConnection();
            con.Open();

            ViewBag.TotalTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks");
            ViewBag.PendingTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Pending'");
            ViewBag.CompletedTasks = GetCount(con, "SELECT COUNT(*) FROM Tasks WHERE Status = 'Completed'");
            ViewBag.AttendanceRecords = GetCount(con, "SELECT COUNT(*) FROM Attendance");

            return View();
        }
    }
}