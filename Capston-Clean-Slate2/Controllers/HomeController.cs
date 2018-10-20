using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("DisplayAllEmployees", "Admins");
            }
            if (User.IsInRole("Employee"))
            {
                return RedirectToAction("EmployeeHome", "Employees");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}