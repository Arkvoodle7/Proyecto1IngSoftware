using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIs.Models;
using APIs.Services;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenCompraController : ControllerBase
    {
        private readonly IOrdenCompraService _ordenCompraService;

        public OrdenCompraController(IOrdenCompraService ordenCompraService)
        {
            _ordenCompraService = ordenCompraService;
        }

        [HttpGet("ordenes")]
        public async Task<ActionResult<List<OrdenCompra>>> ObtenerOrdenesCompra()
        {
            var ordenes = await _ordenCompraService.ObtenerOrdenesCompraAsync();
            return Ok(ordenes);
        }

        [HttpGet("orden")]
        public async Task<ActionResult<List<OrdenCompra>>> ObtenerOrdenCompra(int Num_OC)
        {
            var orden = await _ordenCompraService.ObtenerOCAsync(Num_OC);
            return Ok(orden);
        }

        [HttpPost]
        public async Task<ActionResult<OrdenCompra>> AgregarOC(OrdenCompra oc)
        {
            try
            {
                var nueva_orden = await _ordenCompraService.AgregarOCAsync(oc);
                return Ok("Orden de compra generada con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<OrdenCompra>> ActualizarOC(OrdenCompra oc)
        {
            try
            {
                var orden = await _ordenCompraService.ActualizarOCAsync(oc);
                return Ok("Orden de compra actualizada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{Num_OC}")]
        public async Task<ActionResult<OrdenCompra>> EliminarOC(int Num_OC)
        {
            try
            {
                var orden = await _ordenCompraService.EliminarOCAsync(Num_OC);
                return Ok("Se eliminó la orden de compra");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
