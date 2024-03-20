using EmployeePortal.Data.DataModels.Entities;
using EmployeePortal.Data.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Data.Services.ServiceInterfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync();
        Task<(IEnumerable<EmployeeEntity>, PaginationMetaData)> GetAllEmployeesAsync
            (int? departmentID, string? searchQuery, int pageNumber, int pageSize);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(EmployeeEntity employeeEntity);
        Task DeleteEmployeeAsync(int employeeID);
        Task<bool> IsEmployeeExistsAsync(int employeeID);
        Task<bool> SaveChangesAsync();
    }
}
