


using System.Text.Json;
using backend.Authentication.RoleAccess;
using backend.Models;
using backend.Repositories.Interfaces;

namespace backend.Repositories
{
    internal class JsonProductStatusRepository(string jsonPath, IFilterByRole<ProductStatus> filterer)
    : JsonObjectsRepository<ProductStatus>(jsonPath), IProductStatusRepository, ISeedProductStatusRepository
    {
        public IEnumerable<ProductStatus> PullStatuses(string rol)
        {
            try
            {
                Acquire();
                return filterer.Filter(rol, Load());
            }
            finally
            {
                Release();
            }
        }

        public IEnumerable<ProductStatus> PullStatuses()
        {
            try
            {
                Acquire();
                return Load();
            }
            finally
            {
                Release();
            }

        }

        public ResultValue<bool> SaveStatuses(IEnumerable<ProductStatus> status)
        {
            try
            {
                Acquire();
                Save(status.ToList());
                // TODO: Do some validation
                return new ResultValue<bool>(true);
            }
            finally
            {
                Release();
            }
        }

        public void SeedFile(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "[]");
            }
            List<ProductStatus> list = JsonSerializer.Deserialize<List<ProductStatus>>(File.ReadAllText(file))
                                       ?? throw new JsonException($"Archivo con valor null '{file}' cuando no se esperaba ");
            List<TipoProducto> productoTipos = Enum.GetValues<TipoProducto>().Where(tp => tp != TipoProducto.Base).ToList();
            if (productoTipos.Count != list.Count
                || !productoTipos.All(list.Select(p => p.Tipo).Contains))
            {
                List<ProductStatus> productStatuses = productoTipos.ConvertAll(p => new ProductStatus(p, true));
                File.WriteAllText(file, JsonSerializer.Serialize(productStatuses));
            }
        }
    }
}