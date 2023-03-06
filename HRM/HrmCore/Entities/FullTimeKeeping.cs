using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class FullTimeKeeping : TimeKeeping
    {
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public Guid PositionID { get; set; }
        public Guid DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
    }
}
