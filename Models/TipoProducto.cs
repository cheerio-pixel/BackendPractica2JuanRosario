
using System.Text.Json.Serialization;

namespace backend.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoProducto
    {
        Base = 0,
        Cuenta_de_Ahorro = 1,
        Cuenta_corriente = 2,
        Prestamo = 3,
        Tarjeta_de_credito = 4,
        Certificado = 5
    }
}