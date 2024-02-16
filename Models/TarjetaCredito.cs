
using System.Text.Json.Serialization;

namespace backend.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
    [JsonDerivedType(typeof(TarjetaCredito), nameof(TipoProducto.Tarjeta_de_credito))]
    public class TarjetaCredito : Producto
    {
        public decimal LimiteCredito { get; set; }
        public decimal Saldo { get; set; }

        public TarjetaCredito(Guid id, string name, decimal saldo, decimal limiteCredito)
            : base(id, name, TipoProducto.Tarjeta_de_credito)
        {
            Saldo = saldo;
            LimiteCredito = limiteCredito;
        }
    }

}