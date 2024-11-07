using System.Collections.Generic;
using System.Threading.Tasks;
using APIs.Models;

namespace APIs.Services
{
    public interface IOrdenCompraService
    {
        Task<List<OrdenCompra>> ObtenerOrdenesCompraAsync();
    }
}