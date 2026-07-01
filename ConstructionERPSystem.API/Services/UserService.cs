using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Repositories;

namespace ConstructionERPSystem.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public List<User> GetAllUsers()
        {
            return _repository.GetAllUsers();
        }

        public User GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }

        public void AddUser(User user)
        {
            _repository.AddUser(user);
        }

        public void UpdateUser(User user)
        {
            _repository.UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            _repository.DeleteUser(id);
        }
    }
}