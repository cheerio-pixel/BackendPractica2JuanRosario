
using System.Text.Json.Serialization;
using backend.Utils;

namespace backend.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
    [JsonDerivedType(typeof(Certificado), nameof(TipoProducto.Certificado))]
    public class Certificado : Producto
    {
        public DateOnly FechaVencimiento { get; set; }
        public decimal PrecioDeMaduracion { get; set; }

        public Certificado(Guid id, string name, decimal precioDeMaduracion, DateOnly fechaVencimiento)
            : base(id, name, TipoProducto.Certificado)
        {
            FechaVencimiento = fechaVencimiento;
            PrecioDeMaduracion = precioDeMaduracion;
        }
    }
}