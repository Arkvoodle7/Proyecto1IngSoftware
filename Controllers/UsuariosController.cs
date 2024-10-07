using Microsoft.AspNetCore.Mvc;

namespace Proyecto1IngSoftware.Controllers
{
    public class UsuariosController : Controller
    {
        // Acción para listar los usuarios.
        public IActionResult Index()
        {
            return View();
        }

        // Acción para mostrar el formulario de agregar usuario
        public IActionResult AgregarUsuario()
        {
            return View();
        }

        // Acción para mostrar el formulario de edición de un usuario
        public IActionResult EditarUsuario(int id)
        {
            // Aquí iría la lógica para cargar el usuario con el id proporcionado
            return View();
        }

        // Puedes agregar más acciones según las necesites
    }
}
