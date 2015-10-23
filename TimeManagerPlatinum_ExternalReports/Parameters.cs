using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagerPlatinum_ExternalReports
{
    public class Parameters
    {
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string EmployeeFrom{ get; set; }
        public string EmployeeTo{ get; set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string CostCentreId { get; set; }
        public string GradeId { get; set; }
        public string OccupationId { get; set; }
        public string WorkPatternId { get; set; }
        public bool ZeroValues { get; set; }
        public int AccessVariance { get; set; }
    }
}
