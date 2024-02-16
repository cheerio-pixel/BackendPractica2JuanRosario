

using backend.DTO;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IClientService
    {
        IEnumerable<Client> Search(string? name);
        Client? GetClient(Guid id);
        bool SaveClient(Guid id, ClientProfileDTO client);
        bool DeleteClient(Guid id);
        ResultValue<Guid> CreateClient(ClientProfileDTO client);
        
    }
}