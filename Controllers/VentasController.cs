using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Proyecto1IngSoftware.Models; 

namespace Proyecto1IngSoftware.Controllers
{
    public class VentasController : Controller
    {

        public IActionResult AgregarCliente()
        {
            return View();
        }

        public IActionResult EditarCliente(int id)
        {
            return View();
        }

        private readonly IHttpClientFactory _httpClientFactory;

        public VentasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Cargar la página principal de ventas
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var facturas = new List<FacturaViewModel>();

            try
            {
                // Llamar a la API para obtener las facturas
                var response = await httpClient.GetAsync("https://localhost:7015/api/factura/facturas");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    facturas = JsonSerializer.Deserialize<List<FacturaViewModel>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar las facturas: {ex.Message}");
            }

            return View(facturas);
        }

        // Página de facturación
        [HttpGet]
        public IActionResult Facturar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Facturar(long idCliente, string productosIds)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var productosIdsList = productosIds.Split(',').Select(long.Parse).ToList();

            var ventaRequest = new
            {
                idCliente = idCliente,
                productosIds = productosIdsList
            };

            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(ventaRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7015/api/ventas", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Factura registrada exitosamente.";
                    return RedirectToAction("Index"); // Redirige a la lista de facturas
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    TempData["ErrorMessage"] = "El cliente no fue encontrado. Verifica el ID ingresado.";
                    return View("Facturar");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error en la solicitud: {errorResponse}";
                    return View("Facturar");
                }

                TempData["ErrorMessage"] = "Ocurrió un error inesperado al registrar la factura.";
                return View("Facturar");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar la factura: {ex.Message}");
                TempData["ErrorMessage"] = "Error al conectar con la API. Por favor, intenta nuevamente.";
                return View("Facturar");
            }
        }

    }
}
