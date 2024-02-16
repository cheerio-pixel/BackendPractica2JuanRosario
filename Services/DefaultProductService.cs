
using System.Globalization;
using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services
{
    internal class DefaultProductService
    : IProductService, IProductStatusService, INameProduct
    {
        private readonly IProductStatusRepository _productStatusRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductNameCheck _checker;

        public DefaultProductService(IProductStatusRepository productStatusRepo,
                                     IProductNameCheck checker,
                                     IProductRepository productRepo)
        {
            _productStatusRepo = productStatusRepo;
            _checker = checker;
            _productRepo = productRepo;
        }

        private static string CreateAccountNumber(IProductNameCheck checker)
        {
            bool validAccountNumber;
            string accountNumberCandidate;
            do
            {
                // BTW, this method doesn't support a lot.
                accountNumberCandidate = Random.Shared.Next(100, 1000) + "-" + Random.Shared.Next(100, 1000);
                validAccountNumber = !checker.ProductNameExists(accountNumberCandidate);
            } while (!validAccountNumber);
            return accountNumberCandidate;
        }

        public ResultValue<Guid> AddProductToClient(Guid clientId, Producto product)
        {
            return _productRepo.Create(clientId, product);
        }

        public bool CanCreateOfType(TipoProducto tipo)
        {
            return _productStatusRepo.PullStatuses().First(s => s.Tipo == tipo).IsEnabled;
        }

        public void DeleteProductFromClient(Guid clientId, Guid productId)
        {
            _productRepo.Delete(clientId, productId);
        }

        public string GetNameOf(TipoProducto tipo)
        {
            switch (tipo)
            {
                case TipoProducto.Cuenta_de_Ahorro:
                case TipoProducto.Cuenta_corriente:
                    {
                        return CreateAccountNumber(_checker);
                    }
                case TipoProducto.Prestamo:
                    {
                        string idCandidato;
                        do
                        {
                            idCandidato = Random.Shared.Next(100, 1000)
                                             .ToString(CultureInfo.InvariantCulture)
                                          + "-"
                                          + Random.Shared.Next(1000, 10000)
                                          + "-"
                                          + Random.Shared.Next(1000, 10000)
                                             .ToString(CultureInfo.InvariantCulture);
                        } while (_checker.ProductNameExists(idCandidato));
                        return idCandidato;
                    }
                case TipoProducto.Tarjeta_de_credito:
                    {
                        string idCandidato;
                        do
                        {
                            idCandidato = Random.Shared.NextInt64(100000000000L, 1000000000000L)
                                             .ToString(CultureInfo.InvariantCulture);
                        } while (!Utils.Util.IsValidLuhn(idCandidato.Select(CharUnicodeInfo.GetDecimalDigitValue)
                                                                            .ToArray())
                                 || _checker.ProductNameExists(idCandidato));
                        return idCandidato;
                    }
                case TipoProducto.Certificado:
                    {
                        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        const int length = 18;

                        return new string(Enumerable.Repeat(chars, length)
                            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
                    }
                default: throw new KeyNotFoundException("Don't know how to create name for " + nameof(tipo));
            }
        }

        public IEnumerable<ProductStatus> PullStatuses(string rol)
        {
            return _productStatusRepo.PullStatuses(rol);
        }

        public ResultValue<bool> SaveStatuses(IEnumerable<ProductStatus> status)
        {
            return _productStatusRepo.SaveStatuses(status);
        }

        public Result<Producto> ShowProductOfClient(Guid clientId, Guid productId)
        {
            return _productRepo.Show(clientId, productId);
        }

        public ResultValue<bool> UpdateProductFromClient(Guid clientId, Producto product)
        {
            return _productRepo.Save(clientId, product);
        }
    }
}