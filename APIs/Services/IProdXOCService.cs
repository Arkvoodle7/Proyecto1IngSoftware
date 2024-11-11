using APIs.Models;

namespace APIs.Services
{
    public interface IProdXOCService
    {
        Task<ProdXOC> AgregarProdXOCAsync(ProdXOC prodXoc);
        Task<ProdXOC> EliminarProdXOCAsync(ProdXOC prodXoc);
        Task<List<ProdXOC>> ObtenerProdsXOCAsync(int oc);
    }
}

