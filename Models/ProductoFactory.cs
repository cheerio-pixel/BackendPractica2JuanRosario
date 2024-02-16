

using backend.Services.Interfaces;

namespace backend.Models
{
    internal class ProductoFactory
    : IProductFactory
    {
        private const decimal LIMITE_CREDITO = 100_000;


        private readonly INameProduct _nameBeggar;

        public ProductoFactory(INameProduct nameBeggar)
        {
            _nameBeggar = nameBeggar;
        }

        public Producto CreateTemplate(TipoProducto tipo)
        {
            string name = _nameBeggar.GetNameOf(tipo);
            switch (tipo)
            {
                case TipoProducto.Cuenta_de_Ahorro:
                    return new CuentaAhorro(Guid.NewGuid(), name, .10m, 0);
                case TipoProducto.Cuenta_corriente:
                    return new CuentaCorriente(Guid.NewGuid(), name, 0);
                case TipoProducto.Prestamo:
                    return new Prestamo(Guid.NewGuid(), name, 0);
                case TipoProducto.Tarjeta_de_credito:
                    return new TarjetaCredito(Guid.NewGuid(), name, 0, LIMITE_CREDITO);
                case TipoProducto.Certificado:
                    return new Certificado(Guid.NewGuid(), name, 0, DateOnly.FromDateTime(DateTime.Now));
                default:
                    throw new KeyNotFoundException("No se logro encontrar una plantilla para el tipo producto " + nameof(tipo));
            }
        }
    }
}