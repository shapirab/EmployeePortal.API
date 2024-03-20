using EmployeePortal.Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Data.Services.ServiceInterfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentEntity>> GetAllDepartmentsAsync();
        Task<(IEnumerable<DepartmentEntity>, PaginationMetaData)> GetAllDepartmentsAsync
            (string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<DepartmentEntity?> GetDepartmentByIdAsync(int id);
        Task AddDepartmentAsync(DepartmentEntity entity);
        Task DeleteDepartmentAsync(int departmentID);
        Task<bool> IsDepartmentExistsAsync(int departmentID);
        Task<bool> SaveChangesAsync();
    }
}
