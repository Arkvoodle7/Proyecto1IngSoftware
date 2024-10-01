using Microsoft.AspNetCore.Mvc;

namespace Proyecto1IngSoftware.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
