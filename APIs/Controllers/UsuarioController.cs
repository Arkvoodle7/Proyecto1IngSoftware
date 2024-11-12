using System.Collections.Generic;
using System.Threading.Tasks;
using APIs.Models;
using APIs.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("usuarios")]
        public async Task<ActionResult<List<Usuario>>> ObtenerUsuarios()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObtenerUsuario(long id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> AgregarUsuario(UsuarioRequest usuarioRequest)
        {
            var nuevoUsuario = await _usuarioService.AgregarUsuarioAsync(usuarioRequest);
            return CreatedAtAction(nameof(ObtenerUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarUsuario(UsuarioRequest usuarioRequest)
        {
            var usuarioActualizado = await _usuarioService.ActualizarUsuarioAsync(usuarioRequest);
            return Ok(usuarioActualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(long id)
        {
            var resultado = await _usuarioService.EliminarUsuarioAsync(id);
            if (!resultado)
                return NotFound();
            return Ok("Usuario eliminado exitosamente");
        }
    }
}
