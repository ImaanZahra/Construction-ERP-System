using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Repositories;

namespace ConstructionERPSystem.API.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public List<Client> GetAllClients()
        {
            return _repository.GetAllClients();
        }

        public Client GetClientById(int id)
        {
            return _repository.GetClientById(id);
        }

        public void AddClient(Client client)
        {
            _repository.AddClient(client);
        }

        public void UpdateClient(Client client)
        {
            _repository.UpdateClient(client);
        }

        public void DeleteClient(int id)
        {
            _repository.DeleteClient(id);
        }
    }
}