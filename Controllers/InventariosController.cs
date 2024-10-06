using Microsoft.AspNetCore.Mvc;

namespace Proyecto1IngSoftware.Controllers
{
    public class InventariosController : Controller
    {
        // Método para mostrar el índice de inventarios
        public IActionResult Index()
        {
            return View();
        }

        // Método para mostrar la vista de agregar producto
        public IActionResult AgregarProd()
        {
            return View();
        }

        // Método para mostrar la vista de editar producto
        public IActionResult EditarProd()
        {
            return View();
        }
    }
}

