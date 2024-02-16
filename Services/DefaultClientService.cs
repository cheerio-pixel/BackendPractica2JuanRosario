
using backend.DTO;
using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services
{
    internal class DefaultClientService
    : IClientService
    {
        private readonly IClientRepository _clientRepo;

        public DefaultClientService(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        public ResultValue<Guid> CreateClient(ClientProfileDTO client)
        {
            return _clientRepo.Create(client);
        }

        public bool DeleteClient(Guid id)
        {
            return _clientRepo.Delete(id);
        }

        public Client? GetClient(Guid id)
        {
            return _clientRepo.GetClient(id);
        }

        public bool SaveClient(Guid id, ClientProfileDTO client)
        {
            return _clientRepo.Save(id, client);
        }

        public IEnumerable<Client> Search(string? name)
        {
            return _clientRepo.Search(name);
        }
    }
}