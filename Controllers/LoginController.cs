using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto1IngSoftware.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Usuario, string Password)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var loginRequest = new { Correo = Usuario, Contrasena = Password };

            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://localhost:7015/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var userData = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
                var userRole = userData.GetProperty("rol").GetString();

                // Guardar el rol en la sesión
                HttpContext.Session.SetString("UserRole", userRole);

                return RedirectToAction("Index", "Dashboard");
            }

            TempData["ErrorMessage"] = "Correo o contraseña incorrectos.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Limpia la sesión
            HttpContext.Session.Clear();

            // Redirige al inicio de sesión
            return RedirectToAction("Index", "Login");
        }
    }
}
