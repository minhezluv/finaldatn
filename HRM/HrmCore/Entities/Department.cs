using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Department
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string Address { get; set; }
        public string FaxNumber { get; set; }
       
        public DateTime FoundingDate { get; set; }
        public int Status { get; set; }
        #endregion
    }
}
