using Microsoft.AspNetCore.Mvc;

namespace Proyecto1IngSoftware.Controllers
{
    public class ComprasController : Controller
    {
        // Acción para listar OCs y Proveedores
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AgregarProv()
        {
            return View();
        }

        public IActionResult GenerarOC()
        {
            return View();
        }

        public IActionResult EditarProveedor(int id)
        {
            return View();
        }
        
        public IActionResult EditarOC(int id)
        {
            return View();
        }
    }
}
