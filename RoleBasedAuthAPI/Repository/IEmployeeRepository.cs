using RoleBasedAuthAPI.Dtos;

namespace RoleBasedAuthAPI.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetAllEmployees();
        Task<EmployeeDto> GetEmployeeById(int Id);
        Task<EmployeeDto> CreateEmployess(EmployeeDto employee);
        Task<EmployeeDto> UpdateEmployess(int Id, EmployeeDto employeeDto);
        Task<string> DeleteEmployess(int Id);
    }
}
