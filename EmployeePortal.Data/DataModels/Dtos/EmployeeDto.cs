using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Data.DataModels.Dtos
{
    public class EmployeeDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int DepartmentId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? ProfileImage { get; set; }
    }
}
