using System.Text.Json.Serialization;

namespace backend.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
    [JsonDerivedType(typeof(CuentaCorriente), typeDiscriminator: nameof(TipoProducto.Cuenta_corriente))]
    public class CuentaCorriente : Producto
    {
        public decimal SaldoActual { get; set; }

        public CuentaCorriente(Guid id, string name, decimal saldoActual)
        : base(id, name, TipoProducto.Cuenta_corriente)
        {
            SaldoActual = saldoActual;
        }
    }
}