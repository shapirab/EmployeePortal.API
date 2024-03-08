using EmployeePortal.Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Data.DbContexts
{
    public class EmployeePortalDbContext : DbContext
    {
        public EmployeePortalDbContext(DbContextOptions<EmployeePortalDbContext> options) : base(options)
        {
            
        }

        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
    }
}
