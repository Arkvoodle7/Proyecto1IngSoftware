namespace APIs.Models
{
    public class VentaRequest
    {
        public long IdCliente { get; set; }
        public List<long> ProductosIds { get; set; }
    }
}
