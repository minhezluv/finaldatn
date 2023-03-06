using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class TimeKeeping
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();

        public Guid StaffID { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        #endregion
    }
}
