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
    }
}