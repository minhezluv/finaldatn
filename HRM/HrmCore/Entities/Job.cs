using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Job
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();
        
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }

        #endregion
    }
}
