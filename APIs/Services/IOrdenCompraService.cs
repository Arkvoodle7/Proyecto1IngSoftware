using System.Collections.Generic;
using System.Threading.Tasks;
using APIs.Models;

namespace APIs.Services
{
    public interface IOrdenCompraService
    {
        Task<List<OrdenCompra>> ObtenerOrdenesCompraAsync();
        Task<OrdenCompra> AgregarOCAsync(OrdenCompra oc);
        Task<OrdenCompra> ActualizarOCAsync(OrdenCompra oc);
        Task<OrdenCompra> EliminarOCAsync(int Num_OC);
        Task<List<OrdenCompra>> ObtenerOCAsync(int Num_OC);
    }
}