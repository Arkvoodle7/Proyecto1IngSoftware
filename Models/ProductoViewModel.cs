using System.Text.Json.Serialization;

namespace Proyecto1IngSoftware.Models
{
    public class ProductoViewModel
    {
        public long Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int UnidadesDisponibles { get; set; }

        // Mapea el int de la API a un bool
        [JsonPropertyName("disparador")]
        public bool Disparador
        {
            get => DisparadorInt == 1;
            set => DisparadorInt = value ? 1 : 0;
        }

        [JsonIgnore]
        public int DisparadorInt { get; set; } // Usado internamente
    }

}
