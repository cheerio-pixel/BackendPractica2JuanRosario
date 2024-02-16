
using System.Text.Json.Serialization;

namespace backend.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
    [JsonDerivedType(typeof(Prestamo), nameof(TipoProducto.Prestamo))]
    public class Prestamo : Producto
    {
        public decimal MontoPrestado { get; set; }

        public Prestamo(Guid id, string name, decimal montoPrestado)
            : base(id, name, TipoProducto.Prestamo)
        {
            MontoPrestado = montoPrestado;
        }
    }
}