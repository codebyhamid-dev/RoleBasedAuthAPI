using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthAPI.Dtos;
using RoleBasedAuthAPI.Repository;

namespace RoleBasedAuthAPI.Controllers
{
    [Authorize(Roles = "Emp")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _repository.GetAllEmployees();
            if (employees == null || employees.Count == 0)
            {
                return NotFound("No employees found");
            }
            return Ok(employees);
        }

        [HttpGet("GetEmployeesBy{Id:int}")]
        public async Task<IActionResult> GetEmployeesById(int Id)
        {
            var employees = await _repository.GetEmployeeById(Id);
            if (employees == null)
            {
                return NotFound("No employees found");
            }
            return Ok(employees);
        }

        [HttpPost("CreateEmployees")]
        public async Task<IActionResult> CreateEmployees([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Model is null");
            }
            var employees=await _repository.CreateEmployess(employeeDto);
            return Content("Employee Created Successfully");
        }

        [HttpPut("UpdateEmployees{Id:int}")]
        public async Task<IActionResult> UpdateEmployees(int Id, [FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Model Not Found");
            }

            var existingEmployee = await _repository.GetEmployeeById(Id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {Id} not found.");
            }

            var updatedEmployee = await _repository.UpdateEmployess(Id, employeeDto);
            return Ok(updatedEmployee);
        }

        [HttpDelete("DeleteEmployees{Id:int}")]
        public async Task<IActionResult> DeleteEmployees(int Id)
        {
            var employee=await _repository.DeleteEmployess(Id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
    }
}
