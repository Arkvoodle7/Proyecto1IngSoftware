using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using APIs.Models;
using APIs.Services;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _loginService.ValidarLoginAsync(loginRequest.Correo, loginRequest.Contrasena);
            if (user == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            return Ok(new
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido1 = user.Apellido1,
                Apellido2 = user.Apellido2,
                Rol = user.Rol
            });
        }
    }
}