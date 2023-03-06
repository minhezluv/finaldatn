using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class BasicSalary
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();
        public string BasicSalaryID { get; set; }
        public string StaffID { get; set; }
        public string ApplicationDate { get; set; }
        public float Money { get; set; }
        public string Note { get; set; }
        #endregion
    }
}
