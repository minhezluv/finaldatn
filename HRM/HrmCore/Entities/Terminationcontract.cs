using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Terminationcontract
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();
        public Guid StaffID { get; set; }
        public DateTime Date { get; set; }
        public string Note   { get; set; }
        #endregion
    }
}
