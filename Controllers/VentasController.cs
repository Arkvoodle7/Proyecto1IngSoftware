using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Proyecto1IngSoftware.Models; 

namespace Proyecto1IngSoftware.Controllers
{
    public class VentasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VentasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var facturas = new List<FacturaViewModel>();
            var usuariosClientes = new List<UsuarioViewModel>();

            try
            {
                // Obtener facturas
                var facturasResponse = await httpClient.GetAsync("https://localhost:7015/api/Factura/facturas");
                if (facturasResponse.IsSuccessStatusCode)
                {
                    var facturasJson = await facturasResponse.Content.ReadAsStringAsync();
                    facturas = JsonSerializer.Deserialize<List<FacturaViewModel>>(facturasJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                // Obtener usuarios clientes
                var usuariosResponse = await httpClient.GetAsync("https://localhost:7015/api/Usuario/usuarios");
                if (usuariosResponse.IsSuccessStatusCode)
                {
                    var usuariosJson = await usuariosResponse.Content.ReadAsStringAsync();
                    var allUsuarios = JsonSerializer.Deserialize<List<UsuarioViewModel>>(usuariosJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    usuariosClientes = allUsuarios.Where(u => u.Rol == "Cliente").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar datos: {ex.Message}");
                TempData["ErrorMessage"] = "Error al cargar los datos de la API.";
            }

            // Pasar la tupla a la vista
            return View(Tuple.Create(facturas, usuariosClientes));
        }


        [HttpGet]
        public IActionResult Facturar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Facturar(long idCliente, string productosIds)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var productosIdsList = productosIds.Split(',').Select(long.Parse).ToList();

            var ventaRequest = new
            {
                idCliente = idCliente,
                productosIds = productosIdsList
            };

            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(ventaRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7015/api/ventas", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Factura registrada exitosamente.";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Error al registrar la factura.";
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar la factura: {ex.Message}");
                TempData["ErrorMessage"] = "Error al conectar con la API.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ImprimirFactura(long numFact)
        {
            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                // Obtener datos de factura
                var facturaResponse = await httpClient.GetAsync($"https://localhost:7015/api/ventas/{numFact}");
                if (!facturaResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se pudo obtener la factura.";
                    return RedirectToAction("Index");
                }

                var facturaJson = await facturaResponse.Content.ReadAsStringAsync();
                var factura = JsonSerializer.Deserialize<FacturaViewModel>(facturaJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtener productos relacionados
                var productosResponse = await httpClient.GetAsync($"https://localhost:7015/api/ventas/productosxfactura/{numFact}");
                if (!productosResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se pudieron obtener los productos.";
                    return RedirectToAction("Index");
                }

                var productosJson = await productosResponse.Content.ReadAsStringAsync();
                var productos = JsonSerializer.Deserialize<List<ProductoViewModel>>(productosJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                factura.Productos = productos;

                // Generar PDF
                var pdfBytes = GenerarFacturaPdf(factura);

                return File(pdfBytes, "application/pdf", $"Factura-{numFact}.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar PDF: {ex.Message}");
                TempData["ErrorMessage"] = "Error inesperado.";
                return RedirectToAction("Index");
            }
        }

        private byte[] GenerarFacturaPdf(FacturaViewModel factura)
        {
            using var memoryStream = new System.IO.MemoryStream();
            var document = new Document(PageSize.A4, 36, 36, 54, 54); // Margen
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Encabezado
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            var titleParagraph = new Paragraph("Factura de Venta", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(titleParagraph);

            // Información de la factura
            var infoTable = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingAfter = 20
            };
            infoTable.SetWidths(new float[] { 30, 70 }); // Ancho relativo de columnas

            infoTable.AddCell(new PdfPCell(new Phrase("Factura N°:", regularFont)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(factura.NumFact.ToString(), regularFont)) { Border = Rectangle.NO_BORDER });

            infoTable.AddCell(new PdfPCell(new Phrase("Fecha:", regularFont)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(factura.Fecha.ToShortDateString(), regularFont)) { Border = Rectangle.NO_BORDER });

            infoTable.AddCell(new PdfPCell(new Phrase("Cliente ID:", regularFont)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(factura.IdCliente.ToString(), regularFont)) { Border = Rectangle.NO_BORDER });

            infoTable.AddCell(new PdfPCell(new Phrase("Total:", regularFont)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(factura.Total.ToString("C"), regularFont)) { Border = Rectangle.NO_BORDER });

            document.Add(infoTable);

            // Título de productos
            var productosHeader = new Paragraph("Productos:", titleFont)
            {
                Alignment = Element.ALIGN_LEFT,
                SpacingAfter = 10
            };
            document.Add(productosHeader);

            // Tabla de productos
            var productosTable = new PdfPTable(3)
            {
                WidthPercentage = 100,
                SpacingBefore = 10
            };
            productosTable.SetWidths(new float[] { 50, 25, 25 }); // Ancho relativo de columnas

            // Encabezados de tabla
            productosTable.AddCell(new PdfPCell(new Phrase("Producto", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            productosTable.AddCell(new PdfPCell(new Phrase("Cantidad", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            productosTable.AddCell(new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });

            // Filas de productos
            foreach (var producto in factura.Productos)
            {
                productosTable.AddCell(new PdfPCell(new Phrase(producto.Descripcion, regularFont)) { Padding = 5 });
                productosTable.AddCell(new PdfPCell(new Phrase("1", regularFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER });
                productosTable.AddCell(new PdfPCell(new Phrase(producto.Precio.ToString("C"), regularFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
            }

            document.Add(productosTable);

            // Pie de página
            var footer = new Paragraph("Gracias por su compra", FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 12))
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 20
            };
            document.Add(footer);

            document.Close();

            return memoryStream.ToArray();
        }

    }
}
