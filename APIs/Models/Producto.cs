namespace APIs.Models
{
    public class Producto
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int UnidadesDisponibles { get; set; }
        public int Disparador { get; set; }
    }
}
