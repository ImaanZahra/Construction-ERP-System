using ConstructionERPSystem.Data;
using ConstructionERPSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionERPSystem.Controllers
{
    public class TestController : Controller
    {
        private readonly DbHelper _db;

        public TestController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            using var con = _db.GetConnection();
            con.Open();

            return Content("Database Connected Successfully!");
        }
    }
}