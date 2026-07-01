
using ConstructionERPSystem.Data;
using ConstructionERPSystem.Filters;
using ConstructionERPSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace ConstructionERP.Controllers
{
    [RoleAuthorize(
    "Admin",
    "Accountant",
    "Client"
)]
    public class InvoicesController : Controller
    {
        private readonly DbHelper _db;

        public InvoicesController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Invoice> invoices = new List<Invoice>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT i.*, c.ClientName, p.ProjectName
                FROM Invoices i
                INNER JOIN Clients c ON i.ClientId = c.ClientId
                INNER JOIN Projects p ON i.ProjectId = p.ProjectId";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                invoices.Add(new Invoice
                {
                    InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ClientName = reader["ClientName"].ToString(),
                    ProjectName = reader["ProjectName"].ToString(),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                    InvoiceDate = reader["InvoiceDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["InvoiceDate"]),
                    Status = reader["Status"].ToString()
                });
            }

            return View(invoices);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Invoice invoice)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Invoices
                            (ClientId, ProjectId, TotalAmount, PaidAmount, InvoiceDate, Status)
                            VALUES
                            (@ClientId, @ProjectId, @TotalAmount, @PaidAmount, @InvoiceDate, @Status)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ClientId", invoice.ClientId);
            cmd.Parameters.AddWithValue("@ProjectId", invoice.ProjectId);
            cmd.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
            cmd.Parameters.AddWithValue("@PaidAmount", invoice.PaidAmount);
            cmd.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", invoice.Status ?? "Pending");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Invoice invoice = GetInvoiceById(id);
            if (invoice == null) return NotFound();

            return View(invoice);
        }

        public IActionResult Edit(int id)
        {
            Invoice invoice = GetInvoiceById(id);
            if (invoice == null) return NotFound();

            LoadDropdowns();
            return View(invoice);
        }

        [HttpPost]
        public IActionResult Edit(Invoice invoice)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Invoices
                             SET ClientId = @ClientId,
                                 ProjectId = @ProjectId,
                                 TotalAmount = @TotalAmount,
                                 PaidAmount = @PaidAmount,
                                 InvoiceDate = @InvoiceDate,
                                 Status = @Status
                             WHERE InvoiceId = @InvoiceId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@InvoiceId", invoice.InvoiceId);
            cmd.Parameters.AddWithValue("@ClientId", invoice.ClientId);
            cmd.Parameters.AddWithValue("@ProjectId", invoice.ProjectId);
            cmd.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
            cmd.Parameters.AddWithValue("@PaidAmount", invoice.PaidAmount);
            cmd.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", invoice.Status ?? "Pending");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Invoices WHERE InvoiceId = @InvoiceId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@InvoiceId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private Invoice GetInvoiceById(int id)
        {
            Invoice invoice = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT i.*, c.ClientName, p.ProjectName
                FROM Invoices i
                INNER JOIN Clients c ON i.ClientId = c.ClientId
                INNER JOIN Projects p ON i.ProjectId = p.ProjectId
                WHERE i.InvoiceId = @InvoiceId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@InvoiceId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                invoice = new Invoice
                {
                    InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ClientName = reader["ClientName"].ToString(),
                    ProjectName = reader["ProjectName"].ToString(),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                    InvoiceDate = reader["InvoiceDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["InvoiceDate"]),
                    Status = reader["Status"].ToString()
                };
            }

            return invoice;
        }

        private void LoadDropdowns()
        {
            ViewBag.Clients = GetDropdown("SELECT ClientId, ClientName FROM Clients", "ClientId", "ClientName");
            ViewBag.Projects = GetDropdown("SELECT ProjectId, ProjectName FROM Projects", "ProjectId", "ProjectName");
        }

        private List<SelectListItem> GetDropdown(string query, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using var con = _db.GetConnection();
            con.Open();

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = reader[valueField].ToString(),
                    Text = reader[textField].ToString()
                });
            }

            return list;
        }
    }
}