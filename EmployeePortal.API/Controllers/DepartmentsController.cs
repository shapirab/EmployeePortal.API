using AutoMapper;
using EmployeePortal.Data.DataModels.Dtos;
using EmployeePortal.Data.DataModels.Entities;
using EmployeePortal.Data.DataModels.Models;
using EmployeePortal.Data.Services.Implementations;
using EmployeePortal.Data.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService departmentService;
        private readonly IMapper mapper;
        private readonly int maxPageSize = 20;

        public DepartmentsController(IDepartmentService departmentService, IMapper mapper)
        {
            this.departmentService = departmentService ?? 
                throw new ArgumentNullException(nameof(departmentService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments
            (string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (departmentEntities, paginationMetadata) = await departmentService
                .GetAllDepartmentsAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(mapper.Map<IEnumerable<Department>>(departmentEntities));
        }

        [HttpGet("{departmentID}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int departmentID)
        {
            DepartmentEntity? departmentEntity = 
                await departmentService.GetDepartmentByIdAsync(departmentID);
            if (departmentEntity == null)
            {
                return BadRequest("Departmnet with this id was not found");
            }
            return Ok(mapper.Map<Department>(departmentEntity));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddDepartment(DepartmentDto departmentToAdd)
        {
            DepartmentEntity departmentEntity = mapper.Map<DepartmentEntity>(departmentToAdd);
            await departmentService.AddDepartmentAsync(departmentEntity);
            return Ok(await departmentService.SaveChangesAsync());
        }

        [HttpDelete("{departmentID}")]
        public async Task<ActionResult<bool>> DeleteEmployee(int departmentID)
        {
            DepartmentEntity? departmentEntity = 
                await departmentService.GetDepartmentByIdAsync(departmentID);
            if (departmentEntity == null)
            {
                return BadRequest("Department with this id was not found");
            }
            await departmentService.DeleteDepartmentAsync(departmentID);
            return Ok(await departmentService.SaveChangesAsync());
        }

        [HttpPut("{departmentID}")]
        public async Task<ActionResult> UpdateEmployee(int departmentID, DepartmentDto updatedDepartment)
        {
            DepartmentEntity? departmentEntity = 
                await departmentService.GetDepartmentByIdAsync(departmentID);
            if (departmentEntity == null)
            {
                return BadRequest("Department with this id was not found");
            }
            mapper.Map(updatedDepartment, departmentEntity);
            await departmentService.SaveChangesAsync();
            return NoContent();
        }
    }
}
