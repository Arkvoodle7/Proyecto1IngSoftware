using Microsoft.AspNetCore.Mvc;
using Proyecto1IngSoftware.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

namespace Proyecto1IngSoftware.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7015/api/");
        }

        // Acción para mostrar la vista de Agregar Cliente
        [HttpGet]
        public IActionResult AgregarCliente()
        {
            return View(); // Retorna la vista AgregarCliente.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(UsuarioViewModel cliente)
        {
            cliente.Rol = "Cliente";
            cliente.Contrasena = "defaultPassword123";

            try
            {
                var response = await _httpClient.PostAsJsonAsync("Usuario", cliente);

                if (response.StatusCode == HttpStatusCode.Conflict) // Maneja el error 409 (conflicto)
                {
                    ModelState.AddModelError("", "El cliente ya existe en la base de datos.");
                    return View("AgregarCliente", cliente);
                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cliente registrado exitosamente.";
                    return RedirectToAction("Index", "Ventas");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al registrar cliente. Respuesta de la API: {responseBody}");
                ModelState.AddModelError("", "Error al registrar el cliente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la API: {ex.Message}");
                ModelState.AddModelError("", "Error al conectar con el servidor.");
            }

            return View("AgregarCliente", cliente);
        }

        // Método para mostrar el formulario de edición
        [HttpGet]
        public async Task<IActionResult> EditarCliente(long id)
        {
            try
            {
                var cliente = await _httpClient.GetFromJsonAsync<UsuarioViewModel>($"Usuario/{id}");
                if (cliente == null || cliente.Rol != "Cliente")
                {
                    TempData["ErrorMessage"] = "Cliente no encontrado.";
                    return RedirectToAction("Index", "Ventas");
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el cliente: {ex.Message}");
                TempData["ErrorMessage"] = "Error al cargar los datos del cliente.";
                return RedirectToAction("Index", "Ventas");
            }
        }

        // Método para procesar el formulario de edición
        [HttpPost]
        public async Task<IActionResult> EditarCliente(UsuarioViewModel cliente)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Modelo inválido al editar cliente.");
                return View(cliente);
            }

            try
            {
                cliente.Rol = "Cliente"; // Asegurar que el rol sea "Cliente"
                var response = await _httpClient.PutAsJsonAsync("Usuario", cliente);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cliente actualizado exitosamente.";
                    return RedirectToAction("Index", "Ventas");
                }

                Console.WriteLine($"Error al actualizar cliente. Código de respuesta: {response.StatusCode}");
                TempData["ErrorMessage"] = "Error al actualizar el cliente.";
                return View(cliente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la API: {ex.Message}");
                TempData["ErrorMessage"] = "Error al conectar con el servidor.";
                return View(cliente);
            }
        }

        // Método para eliminar un cliente
        [HttpPost]
        public async Task<IActionResult> EliminarCliente(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Usuario/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cliente eliminado exitosamente.";
                }
                else
                {
                    Console.WriteLine($"Error al eliminar cliente. Código de respuesta: {response.StatusCode}");
                    TempData["ErrorMessage"] = "Error al eliminar el cliente.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la API: {ex.Message}");
                TempData["ErrorMessage"] = "Error al conectar con el servidor.";
            }

            return RedirectToAction("Index", "Ventas");
        }

    }
}

