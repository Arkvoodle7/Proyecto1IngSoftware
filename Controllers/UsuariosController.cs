using Microsoft.AspNetCore.Mvc;
using Proyecto1IngSoftware.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Proyecto1IngSoftware.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7015/api/");
        }

        // Acción para listar los usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioViewModel>>("Usuario/usuarios");
            if (usuarios == null)
            {
                usuarios = new List<UsuarioViewModel>();
            }
            return View(usuarios);
        }

        // Acción para mostrar el formulario de agregar usuario
        [HttpGet]
        public IActionResult AgregarUsuario()
        {
            return View(new UsuarioViewModel());
        }

        // Acción para agregar un usuario (método POST)
        [HttpPost]
        public async Task<IActionResult> AgregarUsuario(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Usuario", usuario);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error al agregar el usuario. Verifica los datos.");
            }
            return View(usuario);
        }

        // Acción para mostrar el formulario de edición de un usuario
        [HttpGet]
        public async Task<IActionResult> EditarUsuario(long id)
        {
            var usuario = await _httpClient.GetFromJsonAsync<UsuarioViewModel>($"Usuario/{id}");
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Acción para editar un usuario (método POST)
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync("Usuario", usuario);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error al actualizar el usuario. Verifica los datos.");
            }
            return View(usuario);
        }

        // Acción para eliminar un usuario
        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(long id)
        {
            var response = await _httpClient.DeleteAsync($"Usuario/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al eliminar el usuario.");
            }
            return RedirectToAction("Index");
        }



    }
}
