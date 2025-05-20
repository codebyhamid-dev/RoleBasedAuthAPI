using System.Data;
using Microsoft.Data.SqlClient;
using RoleBasedAuthAPI.Dtos;

namespace RoleBasedAuthAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<EmployeeDto> CreateEmployess(EmployeeDto employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                using(var command=new SqlCommand("InsertEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                return employee;
            }
        }

        public async Task<string> DeleteEmployess(int Id)
        {
            using (var connection = new SqlConnection(_connectionString)) {
                using (var command = new SqlCommand("DeleteEmployees", connection))
                { 
                    command.CommandType= CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmpID",Id);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                return "Employee Deleted Successfully";
            }
        }

        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            var employees = new List<EmployeeDto>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetAllEmployees", connection))
                {
                    command.CommandType=CommandType.StoredProcedure;
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var employee = new EmployeeDto
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                            };
                            employees.Add(employee);
                        }
                        return employees;
                    }

                }
            }
        }

        public async Task<EmployeeDto> GetEmployeeById(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            { 
                using(var command=new SqlCommand("GetEmployeesById", connection))
                {
                    command.CommandType=CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmpID", Id);
                    await connection.OpenAsync();
                    using(SqlDataReader  reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new EmployeeDto
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                            };
                        }
                    }
                    return null;

                }
            }
        }

        public async Task<EmployeeDto> UpdateEmployess(int Id, EmployeeDto employeeDto)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("UpdateEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("EmpID", Id);
                    command.Parameters.AddWithValue("@Name", employeeDto.Name);
                    command.Parameters.AddWithValue("@Email", employeeDto.Email);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                return employeeDto;
            }
        }
    }
    
}
