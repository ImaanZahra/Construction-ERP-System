
using ConstructionERPSystem.Data;
using ConstructionERPSystem.Filters;
using ConstructionERPSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.Controllers
{
    [RoleAuthorize(
    "Admin",
    "Store Manager",
    "Procurement Officer"
)]
    public class PurchaseOrdersController : Controller
    {
        private readonly DbHelper _db;

        public PurchaseOrdersController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<PurchaseOrder> orders = new List<PurchaseOrder>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT po.*, v.VendorName, m.MaterialName
                FROM PurchaseOrders po
                INNER JOIN Vendors v ON po.VendorId = v.VendorId
                INNER JOIN Materials m ON po.MaterialId = m.MaterialId";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new PurchaseOrder
                {
                    PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                    VendorId = Convert.ToInt32(reader["VendorId"]),
                    MaterialId = Convert.ToInt32(reader["MaterialId"]),
                    VendorName = reader["VendorName"].ToString(),
                    MaterialName = reader["MaterialName"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    OrderDate = reader["OrderDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["OrderDate"]),
                    Status = reader["Status"].ToString()
                });
            }

            return View(orders);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(PurchaseOrder order)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO PurchaseOrders
                            (VendorId, MaterialId, Quantity, TotalAmount, OrderDate, Status)
                            VALUES
                            (@VendorId, @MaterialId, @Quantity, @TotalAmount, @OrderDate, @Status)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@VendorId", order.VendorId);
            cmd.Parameters.AddWithValue("@MaterialId", order.MaterialId);
            cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
            cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", order.Status ?? "Pending");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            PurchaseOrder order = GetOrderById(id);
            if (order == null) return NotFound();

            return View(order);
        }

        public IActionResult Edit(int id)
        {
            PurchaseOrder order = GetOrderById(id);
            if (order == null) return NotFound();

            LoadDropdowns();
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(PurchaseOrder order)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE PurchaseOrders
                             SET VendorId = @VendorId,
                                 MaterialId = @MaterialId,
                                 Quantity = @Quantity,
                                 TotalAmount = @TotalAmount,
                                 OrderDate = @OrderDate,
                                 Status = @Status
                             WHERE PurchaseOrderId = @PurchaseOrderId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@PurchaseOrderId", order.PurchaseOrderId);
            cmd.Parameters.AddWithValue("@VendorId", order.VendorId);
            cmd.Parameters.AddWithValue("@MaterialId", order.MaterialId);
            cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
            cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", order.Status ?? "Pending");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@PurchaseOrderId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private PurchaseOrder GetOrderById(int id)
        {
            PurchaseOrder order = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT po.*, v.VendorName, m.MaterialName
                FROM PurchaseOrders po
                INNER JOIN Vendors v ON po.VendorId = v.VendorId
                INNER JOIN Materials m ON po.MaterialId = m.MaterialId
                WHERE po.PurchaseOrderId = @PurchaseOrderId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@PurchaseOrderId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                order = new PurchaseOrder
                {
                    PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                    VendorId = Convert.ToInt32(reader["VendorId"]),
                    MaterialId = Convert.ToInt32(reader["MaterialId"]),
                    VendorName = reader["VendorName"].ToString(),
                    MaterialName = reader["MaterialName"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    OrderDate = reader["OrderDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["OrderDate"]),
                    Status = reader["Status"].ToString()
                };
            }

            return order;
        }

        private void LoadDropdowns()
        {
            ViewBag.Vendors = GetDropdown("SELECT VendorId, VendorName FROM Vendors", "VendorId", "VendorName");
            ViewBag.Materials = GetDropdown("SELECT MaterialId, MaterialName FROM Materials", "MaterialId", "MaterialName");
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