
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
    "Project Manager",
    "HR Manager",
    "Site Engineer",
    "Worker",
    "Contractor"
)]
    public class AttendanceController : Controller
    {
        private readonly DbHelper _db;

        public AttendanceController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<AttendanceModel> attendanceList = new List<AttendanceModel>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT a.*, e.FullName AS EmployeeName, s.SiteName
                FROM Attendance a
                INNER JOIN Employees e ON a.EmployeeId = e.EmployeeId
                INNER JOIN Sites s ON a.SiteId = s.SiteId";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                attendanceList.Add(new AttendanceModel
                {
                    AttendanceId = Convert.ToInt32(reader["AttendanceId"]),
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    SiteId = Convert.ToInt32(reader["SiteId"]),
                    EmployeeName = reader["EmployeeName"].ToString(),
                    SiteName = reader["SiteName"].ToString(),
                    AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"]),
                    CheckIn = reader["CheckIn"] == DBNull.Value ? null : (TimeSpan)reader["CheckIn"],
                    CheckOut = reader["CheckOut"] == DBNull.Value ? null : (TimeSpan)reader["CheckOut"],
                    Status = reader["Status"].ToString()
                });
            }

            return View(attendanceList);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(AttendanceModel attendance)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Attendance
                            (EmployeeId, SiteId, AttendanceDate, CheckIn, CheckOut, Status)
                            VALUES
                            (@EmployeeId, @SiteId, @AttendanceDate, @CheckIn, @CheckOut, @Status)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@EmployeeId", attendance.EmployeeId);
            cmd.Parameters.AddWithValue("@SiteId", attendance.SiteId);
            cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
            cmd.Parameters.AddWithValue("@CheckIn", attendance.CheckIn ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CheckOut", attendance.CheckOut ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", attendance.Status ?? "Present");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            AttendanceModel attendance = GetAttendanceById(id);
            if (attendance == null) return NotFound();

            return View(attendance);
        }

        public IActionResult Edit(int id)
        {
            AttendanceModel attendance = GetAttendanceById(id);
            if (attendance == null) return NotFound();

            LoadDropdowns();
            return View(attendance);
        }

        [HttpPost]
        public IActionResult Edit(AttendanceModel attendance)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Attendance
                             SET EmployeeId = @EmployeeId,
                                 SiteId = @SiteId,
                                 AttendanceDate = @AttendanceDate,
                                 CheckIn = @CheckIn,
                                 CheckOut = @CheckOut,
                                 Status = @Status
                             WHERE AttendanceId = @AttendanceId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@AttendanceId", attendance.AttendanceId);
            cmd.Parameters.AddWithValue("@EmployeeId", attendance.EmployeeId);
            cmd.Parameters.AddWithValue("@SiteId", attendance.SiteId);
            cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
            cmd.Parameters.AddWithValue("@CheckIn", attendance.CheckIn ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CheckOut", attendance.CheckOut ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", attendance.Status ?? "Present");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Attendance WHERE AttendanceId = @AttendanceId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@AttendanceId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private AttendanceModel GetAttendanceById(int id)
        {
            AttendanceModel attendance = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT a.*, e.FullName AS EmployeeName, s.SiteName
                FROM Attendance a
                INNER JOIN Employees e ON a.EmployeeId = e.EmployeeId
                INNER JOIN Sites s ON a.SiteId = s.SiteId
                WHERE a.AttendanceId = @AttendanceId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@AttendanceId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                attendance = new AttendanceModel
                {
                    AttendanceId = Convert.ToInt32(reader["AttendanceId"]),
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    SiteId = Convert.ToInt32(reader["SiteId"]),
                    EmployeeName = reader["EmployeeName"].ToString(),
                    SiteName = reader["SiteName"].ToString(),
                    AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"]),
                    CheckIn = reader["CheckIn"] == DBNull.Value ? null : (TimeSpan)reader["CheckIn"],
                    CheckOut = reader["CheckOut"] == DBNull.Value ? null : (TimeSpan)reader["CheckOut"],
                    Status = reader["Status"].ToString()
                };
            }

            return attendance;
        }

        private void LoadDropdowns()
        {
            ViewBag.Employees = GetDropdown("SELECT EmployeeId, FullName FROM Employees", "EmployeeId", "FullName");
            ViewBag.Sites = GetDropdown("SELECT SiteId, SiteName FROM Sites", "SiteId", "SiteName");
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