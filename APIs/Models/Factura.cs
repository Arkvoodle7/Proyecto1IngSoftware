namespace APIs.Models
{
    public class Factura
    {
        public long NumFact { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public long IdCliente { get; set; }
    }
}
