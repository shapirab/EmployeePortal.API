using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Data.DataModels.Entities
{
    public class EmployeeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public required DepartmentEntity Department { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? ProfileImage { get; set; }
    }
}
