using Microsoft.AspNetCore.Mvc;

namespace Proyecto1IngSoftware.Controllers
{
    public class VentasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AgregarCliente()
        {
            return View();
        }

        public IActionResult Facturar()
        {
            return View();
        }

        public IActionResult EditarCliente(int id)
        {
            return View();
        }

        public IActionResult EditarFact(int id)
        {
            return View();
        }
    }
}
