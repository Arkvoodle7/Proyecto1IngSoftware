using Microsoft.AspNetCore.Mvc;
using APIs.Models;
using APIs.Services;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _provService;
        public ProveedorController(IProveedorService provService)
        {
            _provService = provService;
        }

        [HttpGet("proveedores")]
        public async Task<ActionResult<List<Proveedor>>> ObtenerProveedores()
        {
            var proveedores = await _provService.ObtenerProveedoresAsync();
            return Ok(proveedores);
        }

        [HttpGet("proveedor")]
        public async Task<ActionResult<List<Proveedor>>> ObtenerProveedor(int id)
        {
            var proveedor = await _provService.ObtenerProveedorAsync(id);
            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<ActionResult<Proveedor>> AgregarProv(Proveedor prov)
        {
            try
            {
                var nuevo_prov = await _provService.AgregarProvAsync(prov);
                return Ok("Proveedor registardo con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Proveedor>> ActualizarProv(Proveedor prov)
        {
            try
            {
                var proveedor = await _provService.ActualizarProvAsync(prov);
                return Ok("Proveedor actualizado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Proveedor>> EliminarProv(int id)
        {
            try
            {
                var proveedor = await _provService.EliminarProvAsync(id);
                return Ok("Proveedor eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
