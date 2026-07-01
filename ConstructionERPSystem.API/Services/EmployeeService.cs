using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Repositories;

namespace ConstructionERPSystem.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public List<Employee> GetAllEmployees() => _repository.GetAllEmployees();

        public Employee GetEmployeeById(int id) => _repository.GetEmployeeById(id);

        public void AddEmployee(Employee employee) => _repository.AddEmployee(employee);

        public void UpdateEmployee(Employee employee) => _repository.UpdateEmployee(employee);

        public void DeleteEmployee(int id) => _repository.DeleteEmployee(id);
    }
}