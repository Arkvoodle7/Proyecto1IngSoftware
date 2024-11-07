namespace APIs.Models
{
    public class OrdenCompra
    {
        public int NumOC { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Aplicada { get; set; }
        public int IdProv { get; set; }
    }
}