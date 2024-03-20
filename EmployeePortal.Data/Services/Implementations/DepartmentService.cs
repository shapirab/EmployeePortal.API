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
    public class DepartmentService : IDepartmentService
    {
        private readonly EmployeePortalDbContext context;

        public DepartmentService(EmployeePortalDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddDepartmentAsync(DepartmentEntity department)
        {
            await context.Departments.AddAsync(department);
        }

        public async Task DeleteDepartmentAsync(int departmentID)
        {
            DepartmentEntity? departmentEntity = await GetDepartmentByIdAsync(departmentID);
            if(departmentEntity != null)
            {
                context.Departments.Remove(departmentEntity);
            }
        }

        public async Task<IEnumerable<DepartmentEntity>> GetAllDepartmentsAsync()
        {
            return await context.Departments.OrderBy(dep => dep.Name).ToListAsync();
        }

        public async Task<(IEnumerable<DepartmentEntity>, PaginationMetaData)> GetAllDepartmentsAsync
            (string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            IQueryable<DepartmentEntity> collection = context.Departments as IQueryable<DepartmentEntity>;
            if(!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                collection = collection.Where(department => department.Name == name);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(department => department.Name.Contains(searchQuery));
            }

            int totalItemCount = await collection.CountAsync();

            PaginationMetaData paginationMetadata = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(department => department.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<DepartmentEntity?> GetDepartmentByIdAsync(int id)
        {
            return await context.Departments.Where(dep => dep.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsDepartmentExistsAsync(int departmentID)
        {
            return await GetDepartmentByIdAsync(departmentID) != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
