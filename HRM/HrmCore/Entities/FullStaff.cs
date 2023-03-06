using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class FullStaff : Staff
    {
        #region props
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid PositionId { get; set; }
        public string PositionName { get; set; }
        public int Status { get; set; }
        #endregion
    }
}
