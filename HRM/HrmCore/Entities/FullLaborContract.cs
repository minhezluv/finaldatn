using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class FullLaborContract:LaborContract
    {
        #region props
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
       // public string TypeLaborContractName { get; set; }
       
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        #endregion
    }
}
