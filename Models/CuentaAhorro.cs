


using System.Text.Json.Serialization;

namespace backend.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
    [JsonDerivedType(typeof(CuentaAhorro), nameof(TipoProducto.Cuenta_de_Ahorro))]
    public class CuentaAhorro : Producto
    {
        public decimal TasaInteres { get; set; }
        public decimal SaldoActual { get; set; }

        public CuentaAhorro(Guid id, string name, decimal tasaInteres, decimal saldoActual)
        : base(id, name, TipoProducto.Cuenta_de_Ahorro)
        {
            TasaInteres = tasaInteres;
            SaldoActual = saldoActual;
        }
    }
}