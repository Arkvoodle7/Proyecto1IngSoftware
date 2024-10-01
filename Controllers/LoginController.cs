using Microsoft.AspNetCore.Mvc;

namespace Proyecto1IngSoftware.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Account/Login.cshtml");
        }
    }
}
