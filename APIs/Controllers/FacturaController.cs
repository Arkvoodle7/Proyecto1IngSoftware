using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIs.Models;
using APIs.Services;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturaController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpGet("facturas")]
        public async Task<ActionResult<List<Factura>>> ObtenerFacturas()
        {
            var facturas = await _facturaService.ObtenerFacturasAsync();
            return Ok(facturas);
        }
    }
}