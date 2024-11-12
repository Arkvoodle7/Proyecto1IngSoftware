using APIs.Models;

namespace APIs.Services
{
    public interface IProveedorService
    {
        Task<List<Proveedor>> ObtenerProveedoresAsync();
        Task<Proveedor> AgregarProvAsync(Proveedor prov);
        Task<Proveedor> ActualizarProvAsync(Proveedor prov);
        Task<Proveedor> EliminarProvAsync(int id);
        Task<List<Proveedor>> ObtenerProveedorAsync(int id);
    }
}
