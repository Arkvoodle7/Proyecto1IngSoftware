namespace APIs.Models
{
    public class Producto
    {
        public long Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int UnidadesDisponibles { get; set; }
        public bool Disparador { get; set; } // Cambiado a bool
    }
}
