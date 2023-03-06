using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Coefficientssalary
    {
        #region props
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string CorfficientSalaryCode { get; set; }
        public string StaffID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int NbOfDayWork { get; set; }
       // public int MyProperty { get; set; }
        #endregion
    }
}
