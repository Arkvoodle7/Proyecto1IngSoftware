using Microsoft.AspNetCore.Mvc;
using APIs.Data;
using APIs.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/ventas
        [HttpPost]
        public async Task<ActionResult<Factura>> CreateVenta([FromBody] VentaRequest ventaRequest)
        {
            // Validar el cliente
            // En VentasController
            var cliente = await _context.Usuarios.FindAsync(ventaRequest.IdCliente);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado");
            }

            // Calcular el total y verificar los productos
            decimal total = 0;
            var productosVendidos = new List<Producto>();

            foreach (var idProducto in ventaRequest.ProductosIds)
            {
                var producto = await _context.Productos.FindAsync(idProducto);
                if (producto == null)
                {
                    return NotFound($"Producto con ID {idProducto} no encontrado");
                }

                if (producto.UnidadesDisponibles <= 0)
                {
                    return BadRequest($"Producto {producto.Descripcion} sin unidades disponibles");
                }

                total += producto.Precio;
                producto.UnidadesDisponibles -= 1;
                productosVendidos.Add(producto);
            }

            // Actualizar los productos
            _context.Productos.UpdateRange(productosVendidos);

            // Crear la factura
            var factura = new Factura
            {
                Fecha = DateTime.Now,
                Total = total,
                IdCliente = ventaRequest.IdCliente
            };
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            // Asociar los productos a la factura
            foreach (var producto in productosVendidos)
            {
                var productoXFact = new ProductoXFact
                {
                    Fact = factura.NumFact,
                    Producto = producto.Codigo
                };
                _context.ProductosXFact.Add(productoXFact);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFactura), new { id = factura.NumFact }, factura);
        }

        // GET: api/ventas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            return factura;
        }
    }
}