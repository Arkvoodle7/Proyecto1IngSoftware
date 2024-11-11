using Microsoft.AspNetCore.Mvc;
using APIs.Models;
using APIs.Services;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdXOCController : ControllerBase
    {
        private readonly IProdXOCService _prodXOCService;

        public ProdXOCController(IProdXOCService prodXOCService)
        {
            _prodXOCService = prodXOCService;
        }

        [HttpPost]
        public async Task<ActionResult<ProdXOC>> AgregarProdXOC(ProdXOC prodXoc)
        {
            try
            {
                var nuevo_producto = await _prodXOCService.AgregarProdXOCAsync(prodXoc);
                return Ok("Producto añadido con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ProdXOC>> EliminarProdXOC(ProdXOC prodXoc)
        {
            try
            {
                var producto = await _prodXOCService.EliminarProdXOCAsync(prodXoc);
                return Ok("Proveedor eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("productos")]
        public async Task<ActionResult<List<ProdXOC>>> ObtenerProdsXOS(int id)
        {
            var productos = await _prodXOCService.ObtenerProdsXOCAsync(id);
            return Ok(productos);
        }
    }
}
