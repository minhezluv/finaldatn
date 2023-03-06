using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Position
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string Note { get; set; }
        #endregion
    }
}
