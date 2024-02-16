

using System.Text.Json.Serialization;
using backend.DTO;

namespace backend.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
    [JsonDerivedType(typeof(CuentaCorriente), typeDiscriminator: nameof(TipoProducto.Cuenta_corriente))]
    [JsonDerivedType(typeof(CuentaAhorro), nameof(TipoProducto.Cuenta_de_Ahorro))]
    [JsonDerivedType(typeof(Prestamo), nameof(TipoProducto.Prestamo))]
    [JsonDerivedType(typeof(Certificado), nameof(TipoProducto.Certificado))]
    [JsonDerivedType(typeof(TarjetaCredito), nameof(TipoProducto.Tarjeta_de_credito))]
    public class Producto
    {
        [JsonInclude]
        public Guid Id { get; set; }
        [JsonInclude]
        public string Name { get; set; }
        [JsonInclude]
        public TipoProducto Kind { get; set; }



        public Producto(Guid id, string name, TipoProducto kind)
        {
            Id = id;
            Name = name;
            Kind = kind;
        }

        #pragma warning disable CS8618
        [JsonConstructor]
        public Producto()
        {
        }
#pragma warning restore CS8618

        public ProductoDTO ToProductoDTO()
        {
            return new ProductoDTO(Id, Name, Kind);
        }
    }
}