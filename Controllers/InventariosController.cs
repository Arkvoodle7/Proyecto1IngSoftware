using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Proyecto1IngSoftware.Models; // Ajusta según tu namespace

namespace Proyecto1IngSoftware.Controllers
{
    public class InventariosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InventariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Página principal de inventarios
        }

        [HttpGet]
        // Vista para agregar un producto
        [HttpGet]
        public IActionResult AgregarProd()
        {
            return View();
        }

        // Acción para procesar la solicitud de agregar un producto
        [HttpPost]
        public async Task<IActionResult> AgregarProducto(long codigo, string descripcion, decimal precio, int unidadesDisponibles, bool disparador)
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Crear el objeto JSON con los datos del producto
            var producto = new
            {
                codigo = codigo,
                descripcion = descripcion,
                precio = precio,
                unidadesDisponibles = unidadesDisponibles,
                disparador = disparador
            };

            // Serializar el objeto JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(producto), Encoding.UTF8, "application/json");

            try
            {
                // Enviar la solicitud POST con el contenido JSON
                var response = await httpClient.PostAsync("https://localhost:7015/api/productos", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Producto agregado exitosamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error de la API: {errorResponse}";
                    Console.WriteLine($"Error de la API: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Excepción: {ex.Message}";
                Console.WriteLine($"Excepción: {ex.Message}");
            }

            return View("AgregarProd");
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerProductos()
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Enviar la solicitud GET a la API
            var response = await httpClient.GetAsync("https://localhost:7015/api/productos");

            if (response.IsSuccessStatusCode)
            {
                var productos = await response.Content.ReadAsStringAsync();
                return Json(JsonSerializer.Deserialize<object>(productos));
            }

            return BadRequest(new { message = "Error al obtener los productos." });
        }
        //solicitudes para editar productos:
        
        [HttpGet]
        public async Task<IActionResult> EditarProd(long id)
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                // Enviar solicitud GET a la API para obtener el producto por su ID
                var response = await httpClient.GetAsync($"https://localhost:7015/api/productos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var producto = await response.Content.ReadAsStringAsync();
                    var productoModel = JsonSerializer.Deserialize<ProductoViewModel>(producto, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(productoModel);
                }

                TempData["ErrorMessage"] = "Error al cargar el producto.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción: {ex.Message}");
                TempData["ErrorMessage"] = "Error al conectar con la API.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditarProducto(ProductoViewModel producto)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var jsonContent = new StringContent(JsonSerializer.Serialize(producto), Encoding.UTF8, "application/json");

            try
            {
                // Enviar solicitud PUT a la API para actualizar el producto
                var response = await httpClient.PutAsync($"https://localhost:7015/api/productos/{producto.Codigo}", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Producto actualizado exitosamente.";
                    return RedirectToAction("Index");
                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Error de la API: {errorResponse}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción: {ex.Message}");
                TempData["ErrorMessage"] = "Error al conectar con la API.";
            }

            return View(producto);
        }
        //solicitud para eliminar un producto

        [HttpPost]
        [Route("Inventarios/EliminarProducto/{id}")]
        public async Task<IActionResult> EliminarProducto(long id)
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                var response = await httpClient.DeleteAsync($"https://localhost:7015/api/productos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Producto eliminado exitosamente." });
                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                return BadRequest(new { message = $"Error al eliminar el producto: {errorResponse}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción: {ex.Message}");
                return StatusCode(500, new { message = "Error al conectar con la API." });
            }
        }

    }
}
