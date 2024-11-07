namespace APIs.Models
{
    public class Factura
    {
        public int NumFact { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int IdCliente { get; set; }
    }
}