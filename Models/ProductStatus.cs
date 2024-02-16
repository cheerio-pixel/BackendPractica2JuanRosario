using System.Text.Json.Serialization;

namespace backend.Models
{
    public class ProductStatus
    {
        [JsonInclude]
        public TipoProducto Tipo { get; set; }

        [JsonInclude]
        public bool IsEnabled { get; set; }
        public ProductStatus(TipoProducto tipo, bool isEnabled)
        {
            Tipo = tipo;
            IsEnabled = isEnabled;
        }
    }
}