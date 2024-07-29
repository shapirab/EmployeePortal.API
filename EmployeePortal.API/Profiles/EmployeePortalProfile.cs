using AutoMapper;
using EmployeePortal.Data.DataModels.Dtos;
using EmployeePortal.Data.DataModels.Entities;
using EmployeePortal.Data.DataModels.Models;

namespace EmployeePortal.API.Profiles
{
    public class EmployeePortalProfile : Profile
    {
        public EmployeePortalProfile()
        {
            CreateMap<EmployeeEntity, Employee>();
            CreateMap<Employee, EmployeeEntity>();
            CreateMap<EmployeeEntity, EmployeeDto>();
            CreateMap<EmployeeDto, EmployeeEntity>();
            
            CreateMap<DepartmentEntity, Department>();
            CreateMap<Department, DepartmentEntity>();
            CreateMap<DepartmentEntity, DepartmentDto>();
            CreateMap<DepartmentDto, DepartmentEntity>();
        }
    }
}
