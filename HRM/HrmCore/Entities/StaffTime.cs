using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class StaffTime : TimeKeeping
    {
        #region props
        
        public List<DateTime> TimeIn { get; set; }
        public List<DateTime> TimeOut { get; set; }
        #endregion
    }
}
