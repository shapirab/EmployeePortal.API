using EmployeePortal.Data.DataModels.Entities;
using EmployeePortal.Data.DbContexts;
using EmployeePortal.Data.Services.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Data.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeePortalDbContext context;
        private readonly IDepartmentService departmentService;

        public EmployeeService(EmployeePortalDbContext context, IDepartmentService departmentService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        }

        public async Task AddEmployeeAsync(EmployeeEntity employeeEntity)
        {
            DepartmentEntity? departmentEntity = 
                await departmentService.GetDepartmentByIdAsync(employeeEntity.DepartmentId);
            if(departmentEntity != null)
            {
                departmentEntity.Employees.Add(employeeEntity);
            }
        }

        public async Task DeleteEmployeeAsync(int employeeID)
        {
            EmployeeEntity? employeeEntity = await GetEmployeeByIdAsync(employeeID);
            if(employeeEntity != null)
            {
                context.Employees.Remove(employeeEntity);
            }
        }

        public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync()
        {
            return await context.Employees.OrderBy(emp => emp.LastName).ToListAsync();
        }

        public async Task<(IEnumerable<EmployeeEntity>, PaginationMetaData)> GetAllEmployeesAsync
            (int? departmentID, string? searchQuery, int pageNumber, int pageSize)
        {
            IQueryable<EmployeeEntity> collection = context.Employees as IQueryable<EmployeeEntity>;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(emp => emp.FirstName.Contains(searchQuery) || 
                    emp.LastName.Contains(searchQuery));
            }
            if(departmentID != null)
            {
                collection = collection.Where(emp => emp.DepartmentId == departmentID);
            }

            int totalItemCount = await collection.CountAsync();

            PaginationMetaData paginationMetadata = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(emp => emp.LastName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int id)
        {
            return await context.Employees.Where(emp => emp.EmployeeId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsEmployeeExistsAsync(int employeeID)
        {
            return await GetEmployeeByIdAsync(employeeID) != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
