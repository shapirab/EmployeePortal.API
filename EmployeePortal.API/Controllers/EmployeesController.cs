using AutoMapper;
using EmployeePortal.Data.DataModels.Dtos;
using EmployeePortal.Data.DataModels.Entities;
using EmployeePortal.Data.DataModels.Models;
using EmployeePortal.Data.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;
        private readonly int maxPageSize = 20;

        public EmployeesController(IEmployeeService employeeService, IMapper mapper)
        {
            this.employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees
            (int? departmentFilter, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (employeeEntities, paginationMetadata) = await employeeService
                .GetAllEmployeesAsync(departmentFilter, searchQuery, pageNumber, pageSize);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));


            return Ok(mapper.Map<IEnumerable<Employee>>(employeeEntities));
        }

        [HttpGet("{employeeID}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int employeeID)
        {
            EmployeeEntity? employeeEntity = await employeeService.GetEmployeeByIdAsync(employeeID);
            if(employeeEntity == null)
            {
                return NotFound("Employee with this id was not found");
            }
            return Ok(mapper.Map<Employee>(employeeEntity));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddEmployee(Employee employee)
        {
            EmployeeEntity employeeEntity = mapper.Map<EmployeeEntity>(employee);
            await employeeService.AddEmployeeAsync(employeeEntity);
            return Ok(await employeeService.SaveChangesAsync());
        }

        [HttpDelete("{employeeID}")]
        public async Task<ActionResult<bool>> DeleteEmployee(int employeeID)
        {
            EmployeeEntity? employeeEntity = await employeeService.GetEmployeeByIdAsync(employeeID);
            if(employeeEntity == null)
            {
                return BadRequest("Employee with this id was not found");
            }
            await employeeService.DeleteEmployeeAsync(employeeID);
            return Ok(await employeeService.SaveChangesAsync());
        }

        [HttpPut("{employeeID}")]
        public async Task<ActionResult>UpdateEmployee(int employeeID, EmployeeDto updatedEmployee)
        {
            EmployeeEntity? employeeEntity = await employeeService.GetEmployeeByIdAsync(employeeID);
            if (employeeEntity == null)
            {
                return BadRequest("Employee with this id was not found");
            }
            mapper.Map(updatedEmployee, employeeEntity);
            await employeeService.SaveChangesAsync();
            return NoContent();
        }
    }
}
