

using backend.DTO;
using backend.Models;

namespace backend.Repositories.Interfaces
{
    internal interface IClientRepository
    {
        IEnumerable<Client> Search(string? name);
        Client? GetClient(Guid id);
        bool Save(Guid id, ClientProfileDTO client);
        bool Delete(Guid id);
        ResultValue<Guid> Create(ClientProfileDTO client);
    }
}