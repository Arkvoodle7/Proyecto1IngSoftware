namespace Proyecto1IngSoftware.Models
{
    public class FacturaViewModel
    {
        public long NumFact { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public long IdCliente { get; set; }
        public List<ProductoViewModel> Productos { get; set; }
    }

}
